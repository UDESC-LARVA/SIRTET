from pickle import FALSE
import numpy as np

import cv2
import mediapipe as mp
import time
import socket



########
# MediaPipe for SIRTET K3D
# Diego Fellipe Tondorf 
# Version 2.21
# Last Edit: 31/08/2023
########

###Connection info
localIP     = "0.0.0.0"
localPort   = 20001
bufferSize  = 1024
clientAddress = '127.0.0.1', 8051    

# Create a datagram socket
UDPServerSocket = socket.socket(family=socket.AF_INET, type=socket.SOCK_DGRAM)
# Bind to address and ip
UDPServerSocket.bind((localIP, localPort))
print("UDP server up and listening")
# Listen for incoming datagrams
###

###Mediapipe solutions
mp_drawing = mp.solutions.drawing_utils
mp_pose = mp.solutions.pose

#Index of desired points
index = [0,19,20,14,13,11,12,23,24,26,25,30,29]

#For camera (True) / video (False) control
cameraMode = false
#Change to False in case of file test
videoDir = "Videos/test3.mp4"
###

###Landmark Suavization
lastPoseLandmarks = []

###Values Estimation
#Limit of values of the lists used for estimation
ListLimit = 100 

#List of the lowest foot point
yList = []
#Estimation of the height of the floor
floorY = 0

#List of estimated person height
heightList  = []
#Estimation of the person height
estHeight = 0

#Highest jump done
highestJump = 0
#proportion according to the height of the person
jumpProp = 0
###

#FPS Show
prev_frame_time = 0
new_frame_time = 0


#######FUNCTIONS#######
#Remove the background from the image
def RemoveBackground(_frame):
    annotated_image = _frame.copy()
    condition = np.stack((results.segmentation_mask,) * 3, axis=-1) > 0.1
    bg_image = np.zeros(_frame.shape, dtype=np.uint8)
    bg_image[:] = (100,200,150)
    annotated_image = np.where(condition, annotated_image, bg_image)
    return annotated_image

#Print landmarks in the frame
def PrintLandmarks(_frame, _landmarks):
    for i in index:
        x1 = int(_landmarks[i].x * width)
        y1 = int(_landmarks[i].y * height)
        viz = _landmarks[i].visibility
        color = int(255-(255*viz))
        cv2.circle(_frame, (x1, y1), 3, (color,color,color), 9)
    return _frame 

###Get Functions
#Get offset from center, to be able to move sideways
def GetXOffset(_landmarks):
    return (((_landmarks[23].x + _landmarks[24].x)/2)-(0.5)) * 1500

#Get lower foot id
def GetLowerFootId(_landmarks):
    return 29 if  (_landmarks[29].y>_landmarks[30].y) else 30

#Get floor y estimative from landmarks
def GetFloorY(_yList, _lowerFootY):
    _yList.insert(0, _lowerFootY)
                    
    if len(yList)>ListLimit:
       yList.pop(ListLimit)
    
    sum = 0
    for x in _yList:
        sum = sum + x
    
    return sum/len(_yList)   

def CalculeEstHeightFrame(_world_landmarks, _lowerFootId):           
    #cleaner, faster but less accurate version
    _estHeight = _world_landmarks[_lowerFootId].y + abs(_world_landmarks[0].y) + (_world_landmarks[12].y - _world_landmarks[0].y) 

    return _estHeight

#Get heigth estimative from world landmarks
def GetEstHeight(_heightList, _estHeightFrame):

    _heightList.insert(0, _estHeightFrame)
    
    if len(_heightList)>ListLimit:
        _heightList.pop(ListLimit)
    
    sum = 0
    for x in _heightList:
        sum = sum + x
    
    return round(sum/len(_heightList),3) 

#Final corrections of data to SIRTET
def LandmaksCorrections(_world_landmarks, _xOffset, _feetY, _yOffSet):
    dist = 1000
    for i in index:
        _world_landmarks[i].x = (_world_landmarks[i].x * dist) + _xOffset
        _world_landmarks[i].y = ((_world_landmarks[i].y - _feetY) * dist * -1) - 950 - _yOffSet
        _world_landmarks[i].z = (_world_landmarks[i].z * dist) + 2500
    return _world_landmarks 

#Landmarks Suavization to stop flickering
def LandmarksSuavization(_world_landmarks, _lastPoseLandmarks):
    if len(_lastPoseLandmarks)==0:
        return _world_landmarks

    for x in range(len(_world_landmarks)):
        _world_landmarks[x].x = (_world_landmarks[x].x+_lastPoseLandmarks[x].x) / 2
        _world_landmarks[x].y = (_world_landmarks[x].y+_lastPoseLandmarks[x].y) / 2
        _world_landmarks[x].z = (_world_landmarks[x].z+_lastPoseLandmarks[x].z) / 2
    
    return _world_landmarks


#Get the text from landmark 
def GetTextFromLandmark(_landmark):

    text = str(int(_landmark.x))
    text = text +  ";"
    text = text + str(int(_landmark.y))
    text = text +  ";"
    text = text + str(int(_landmark.z))
    text = text +  ";"

    return text

#Create the final string to send to the client
def GetMsgFromServer(_world_landmarks):

    msgFromServer =""
    #15                  |Mao Esquerda       | [0] - [2]
    msgFromServer = msgFromServer + GetTextFromLandmark(_world_landmarks[20])   
    #16                  |Mao Direita        | [3] - [5]
    msgFromServer = msgFromServer + GetTextFromLandmark(_world_landmarks[19])   
    #0                   |Cabeca             | [6] - [8]
    msgFromServer = msgFromServer + str(int(_world_landmarks[0].x))
    msgFromServer = msgFromServer +  ";"
    msgFromServer = msgFromServer + str(int(_world_landmarks[0].y))
    msgFromServer = msgFromServer +  ";"
    msgFromServer = msgFromServer + str(int((_world_landmarks[11].z + _world_landmarks[12].z)/2))            
    msgFromServer = msgFromServer +  ";"
    #27                  |Pe Esquerdo        | [9] - [11]
    msgFromServer = msgFromServer + GetTextFromLandmark(_world_landmarks[30])
    #28                  |Pe Direito         | [12] - [14]
    msgFromServer = msgFromServer + GetTextFromLandmark(_world_landmarks[29])
    #25                  |Joelho Esquerdo    | [15] - [17]
    msgFromServer = msgFromServer + GetTextFromLandmark(_world_landmarks[26])
    #26                  |Joelho Direito     | [18] - [20]
    msgFromServer = msgFromServer + GetTextFromLandmark(_world_landmarks[25])
    #24                  |Cintura Direita    | [21] - [23]
    msgFromServer = msgFromServer + GetTextFromLandmark(_world_landmarks[23])
    #23                  |Cintura Esquerda   | [24] - [26]
    msgFromServer = msgFromServer + GetTextFromLandmark(_world_landmarks[24])
    #11                  |Ombro Direito      | [27] - [29]
    msgFromServer = msgFromServer + GetTextFromLandmark(_world_landmarks[12])
    #12                  |Ombro Esquerdo     | [30] - [32]
    msgFromServer = msgFromServer + GetTextFromLandmark(_world_landmarks[11])
    #13                  |Cotovelo Esquerdo  | [33] - [35]
    msgFromServer = msgFromServer + GetTextFromLandmark(_world_landmarks[14])
    #14                  |Cotovelo Direito   | [36] - [38]
    msgFromServer = msgFromServer + GetTextFromLandmark(_world_landmarks[13])    
    #MEIO DO 11 E 12     |pescoco            | [39] - [41]
    msgFromServer = msgFromServer + str(int(((_world_landmarks[11].x + _world_landmarks[12].x)/2)))
    msgFromServer = msgFromServer +  ";"
    msgFromServer = msgFromServer + str(int(((_world_landmarks[11].y + _world_landmarks[12].y)/2)))
    msgFromServer = msgFromServer +  ";"
    msgFromServer = msgFromServer + str(int(((_world_landmarks[11].z + _world_landmarks[12].z)/2)))
    msgFromServer = msgFromServer +  ";"
        
    return msgFromServer

#Send message from server to client
def SendMsgFromServer(_msgFromServer):
    bytesToSend         = str.encode(_msgFromServer)
    UDPServerSocket.sendto(bytesToSend, clientAddress)
    return True

#To info visualizations
def PrintInFrame(_frame, value, count):    
    font = cv2.FONT_HERSHEY_SIMPLEX    
    org = (10, count * 30)    
    fontScale = 1    
    color = (255, 0, 0)    
    thickness = 2
    
    return cv2.putText(_frame, str(value), org, font, fontScale, color, thickness, cv2.LINE_AA)

#######FUNCTIONS#######
#####Debug functions
test1 = test2 = ""
def DB(t, v):#debug, this, variable
    return str(t) + " | " + str(v)

def PrintTest():
    print(str(test1) + " | " +str(test2) + " | "+ str(test1 == test2) + " |")
######






with mp_pose.Pose  (
    static_image_mode = False,
    model_complexity = 1,
    min_detection_confidence = 0.5,
    min_tracking_confidence = 0.5,
    enable_segmentation = True) as pose:

    if cameraMode: 
        cap = cv2.VideoCapture(0, cv2.CAP_DSHOW)
    else:
        cap = cv2.VideoCapture(videoDir)

    while True:
        ret, frame = cap.read()        

        if ret == False:
            if cameraMode == False:
                cap.set(cv2.CAP_PROP_POS_FRAMES, 0)
                ret, frame = cap.read()

                if ret:
                    print("End of video, repeating")
                else:
                    print("Video ERROR, check dir: " + str(videoDir))
                    print("If you want Camera chance cameraMode to True")
                    input()     
                    break   
            else:
                print("Camera ERROR, check if camera is being used or change cap = cv2.VideoCapture(0, cv2.CAP_DSHOW) to cap = cv2.VideoCapture(1, cv2.CAP_DSHOW)")
                input()
                break

        frame = cv2.flip(frame, 1)
        height, width, _ = frame.shape
        frame_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        results = pose.process(frame_rgb)
        
        if results.pose_landmarks:
            try: 
                landmarks = results.pose_landmarks.landmark
                world_landmarks = results.pose_world_landmarks.landmark
            except:
                pass            
                
            ####### Data correction for SIRTET            
            #Get offset from center, to be able to move sideways
            xOffset = GetXOffset(landmarks)

            #Get lower foot id
            lowerFootId = GetLowerFootId(landmarks)

            #### Start of Jump
            #Get lower foot y from landmarks
            lowerFootY = float(landmarks[lowerFootId].y)

            #Get floot y estimative from landmarks
            floorY = GetFloorY(yList, lowerFootY)
           
            #Calculate the estimated height value of the person in this frame
            estHeightFrame = CalculeEstHeightFrame(world_landmarks, lowerFootId)

            #Get height estimative from world landmarks
            estHeight = GetEstHeight(heightList, estHeightFrame)
            
            #Get the difference between the lowest foot and the floor
            yOffSet = (lowerFootY - floorY) * 3500            

            #Smoothes the variation to not consider jumping every frame
            yOffSetVariation = abs(yOffSet) - 51  

            if yOffSetVariation < 0:
                yOffSet = 0
            #Base height y for the jump
                jumpbaseY = 0
            #If the variation is large enough, consider a jump
            else:
                if jumpbaseY == 0:
                    jumpbaseY = world_landmarks[lowerFootId].y   

            #Checks and stores the values of the current jump and the highest jump
            if yOffSet != 0:
                currJump = jumpbaseY - world_landmarks[lowerFootId].y
                if currJump > highestJump:
                    highestJump = round(currJump, 3)
                    jumpProp = round(highestJump / estHeight * 100, 3)
            #### End of Jump           

            #Get the lower foot y, from world to data corretion
            feetY = world_landmarks[lowerFootId].y 

            #Final corrections of data to SIRTET
            world_landmarks = LandmaksCorrections(world_landmarks, xOffset, feetY, yOffSet)

            #Landmarks Suavization to stop flickering
            world_landmarks = LandmarksSuavization(world_landmarks, lastPoseLandmarks)

            lastPoseLandmarks = list(world_landmarks)            
            ###### End of Data Corrections            
                                                     
            #Create the final string to send to the client
            msgFromServer = GetMsgFromServer(world_landmarks)

            #Send message from server to client
            SendMsgFromServer(msgFromServer)

            ####### Start of vizualizations
            #Remove the background from the image
            frame = RemoveBackground(frame)     

            #Print landmarks in the frame
            frame = PrintLandmarks(frame, landmarks)
            
            frame = PrintInFrame(frame, "Altura: " + str(estHeight), 1)            
            frame = PrintInFrame(frame, "Pulo: " + str(highestJump), 2)            
            frame = PrintInFrame(frame, "%: " + str(jumpProp), 3)

            ###FPS SHOW
            new_frame_time = time.time()
            fps = int(1/(new_frame_time-prev_frame_time))
            prev_frame_time = new_frame_time
            frame = PrintInFrame(frame, "FPS: " + str(fps), 4)

            cv2.line(frame, (0, int(lowerFootY * height)), (width, int(lowerFootY * height)), 255, 2) 
            cv2.line(frame, (0, int(floorY * height)), (width, int(floorY * height)), 255, 1)            
            ########## End of vizualizations

        cv2.imshow("Frame", frame)

        if(cameraMode == True):
            time.sleep(1/30)

        if cv2.waitKey(1) & 0xFF == 27:
            bytesToSend         = str.encode("Conexao finalizada")
            UDPServerSocket.sendto(bytesToSend, clientAddress)
            break
        
cap.release()
cv2.destroyAllWindows()
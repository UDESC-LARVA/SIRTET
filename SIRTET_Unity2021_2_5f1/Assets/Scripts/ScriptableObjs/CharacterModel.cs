using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "00_Nome", menuName = "Novo personagem")]
public class CharacterModel : ScriptableObject
{
    public string nome;
    public Mesh antebracoDir;
    public Mesh antebracoEsq;
    public Mesh bracoDir;
    public Mesh bracoEsq;
    public Mesh cabeca;
    public Mesh coxaDir;
    public Mesh coxaEsq;
    public Mesh pernaDir;
    public Mesh pernaEsq;

    public Material[] materials;
}

using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    [BoxGroup("Sources"), SerializeField] private AudioSource footR;
    [BoxGroup("Sources"), SerializeField] private AudioSource footL;
    // Start is called before the first frame update
    public void Foot_L()
    {
        
    }

    public void Foot_R()
    {

    }

    protected void EmitFootSound()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Peices {
    Empty,
    Pawn,
    Rook,
    Knight,
    Bishop,
    Queen,
    King,
    
}




public class PlayerPeice : MonoBehaviour
{

    public static string[] meshPath = { "Mesh/Chess_Centered_Pawn", "Mesh/Chess_Centered_Rook", "Mesh/Chess_Centered_Knight", "Mesh/Chess_Centered_Bishop", "Mesh/Chess_Centered_Queen", "Mesh/Chess_Centered_King" };
    public GameObject mesh;
    public Peices peiceType;
    public int owner = 1;

    public float lerpPercent = .01f;

    public Vector2 acessIndex;

    public bool animateHover = false;
    public bool animateMove = false;
    public bool animateGround = false;


    public Vector3 hoverTarget;
    public Vector3 groundedTarget;
    public Vector3 moveTarget;


    void Start() {
        hoverTarget = transform.position;
        groundedTarget = transform.position;
        moveTarget = transform.position;
        

    }

    public void init(Transform location, Peices peice, Material mat, int owner){
        peiceType = peice;
        this.owner = owner;
        Quaternion rot = (owner == 1) ? Quaternion.identity : Quaternion.Euler(0,180,0);
        //print(mat.name);
        switch (peice)
        {
            case Peices.Pawn:
                HandleInstantiateMesh(mat, rot, Peices.Pawn);
                break;
            case Peices.Rook:
                HandleInstantiateMesh(mat, rot, Peices.Rook);
                break;
            case Peices.Knight:
                HandleInstantiateMesh(mat, rot, Peices.Knight);
                break;
            case Peices.Bishop:
                HandleInstantiateMesh(mat, rot, Peices.Bishop);
                break;
            case Peices.Queen:
                HandleInstantiateMesh(mat, rot, Peices.Queen);
                break;
            case Peices.King:
                HandleInstantiateMesh(mat, rot, Peices.King);
                break;
            default:
                
                break;
        }



    }



    void Update() {

        if (animateHover)
        {
            transform.position = Vector3.Lerp(transform.position, hoverTarget, lerpPercent);

            if (transform.position == hoverTarget)
            {
                animateHover = false;
            }

        }
        else if (animateMove)
        {
            transform.position = Vector3.Lerp(transform.position, moveTarget, lerpPercent);
            
            if (transform.position == moveTarget) {
                animateMove = false;
            }

        }
        else if (!animateHover && transform.position != groundedTarget)
        {
            transform.position = Vector3.Lerp(transform.position, groundedTarget, lerpPercent);
        }
    
    
    }

    private void HandleInstantiateMesh(Material mat, Quaternion rot, Peices peice)
    {
        mesh = InstantiateMesh(meshPath[(int)peice-1], transform, rot);
        mesh.AddComponent<CapsuleCollider>();
        mesh.layer = LayerMask.NameToLayer("Peices");

        //print(mesh.layer);

       MeshRenderer matAcess =  mesh.GetComponent<MeshRenderer>();
        // print("Before Name: " + matAcess.materials[1].name);
        if (matAcess != null)
        {
            //print("we got acess bois");
            matAcess.material = mat;
        }

    }


    public void HoverUp(Vector3 hoverTarget) {
        animateHover = true;
        this.hoverTarget = hoverTarget; 

    }

    public void HoverDown(Vector3 groundedTarget) {
        animateGround = true;
        this.groundedTarget = groundedTarget; 

        
    }

    public void MovePeice(Vector3 moveTarget, Vector3 groundedTarget) {
        animateMove = true;
        this.moveTarget = moveTarget;
        this.groundedTarget = groundedTarget;
        
    }




    /*
    public GameObject InstantiateMesh(string path, Transform pos)
    {

        return InstantiateMesh(path, pos, Quaternion.identity);

    }
    */



    public GameObject InstantiateMesh(string path, Transform parent, Quaternion rotation)
    {

        return Instantiate(Resources.Load<GameObject>(path), parent.position, rotation,parent) as GameObject;

    }



}

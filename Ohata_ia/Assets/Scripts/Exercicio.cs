using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exercicio : MonoBehaviour
{

    //Atribui��o das vari�veis

    [Header("Properties")]

    [SerializeField] float force;
    Rigidbody rb;
    Collider playerCollider;

    [Header("OneTarget")]

    [SerializeField] Transform point;

    [Header("Multiple Targets")]

    [SerializeField] bool multipleTargets;
    [SerializeField] Transform[] targets;
    [SerializeField] int targetIndex = 0;

    [SerializeField] Material changeMaterial;

    private void Start()
    {
        //Coletando as referencias para o RB e Collisor
        rb = gameObject.GetComponent<Rigidbody>();
        playerCollider = gameObject.GetComponent<Collider>();
    }
    private void Update()
    {
        // Verifica se a bool para multiplos alvos est� ativada ou n�o
        if (!multipleTargets) // se n�o for ativo a bool para v�rios alvos ir� entrar neste if
        {
            if (Vector3.Distance(this.transform.position, point.position) >= .05f) // verifica se a posi��o do alvo � maior que 0.5
            {
                MoveObject(point); // se for maior, ir� chamar esse m�todo, que recebe como par�metro o transform do alvo
            }
            else
            {
                DisablePhysics(); // caso a distancia for menor que 0.5, ir� chamar esse m�todo que ir� desativar a f�sica do script RB
            }
        }
        // Verifica se o contador est� menor que o tamanho do array pra n�o dar erro.
        else if (targetIndex < targets.Length) // dentro deste if, apenas ir� rodar, caso seja multiplos alvos
        {
            // assim como no alvo �nico, verifica a distancia do gameobject e do alvo.
            if (Vector3.Distance(this.transform.position, MoveToTargets(targetIndex).position) >= .2f)
            {
                // caso entre neste m�todo, ir� chamar o MoveObject que � respons�vel pela movimenta��o, e como par�metro, recebe o transform do alvo atual (index)
                MoveObject(MoveToTargets(targetIndex));
            }
        }
        // Caso alcance = Contador < Tamanho do array, ir� entrar nesse else
        else
        {
            //Caso chegue na ultima posi��o, ele ir� em dire��o ao pen�ltimo ponto.
            if (Vector3.Distance(this.transform.position, MoveToTargets(targetIndex - 2).position) >= .05f)
            {
                MoveObject(MoveToTargets(targetIndex - 2)); // ir� se mover at� o pen�ltimo alvo.
            }
            else
            {
                DisablePhysics();
            }
        }

    }
    
    private Transform MoveToTargets(int actualTarget) // m�todo para fazer refer�ncia ao index atual, devolvendo assim o transform do alvo atual (index)
    {
        return targets[actualTarget]; // faz o retorno.
    }
    private void MoveObject(Transform target) // m�todo para fazer a movimenta��o do gameObject
    {
        Vector3 targetVector = (target.position - this.gameObject.transform.position).normalized; // targetVector, um vetor que sempre aponta para o target
        rb.AddForce(targetVector * force, ForceMode.Force); // AddForce, aplicamos a for�a no RB para que possamos fazer a movimenta��o at� o target, esta for�a � baseado no force
    }
    private void DisablePhysics() // desativa a f�sica quando se aproxima do alvo para evitar o gameObject ficar tremendo/orbitar em volta
    {
        playerCollider.enabled = false; // desativa o colisor, caso haja mais de um com o mesmo alvo, acaba evitando problemas de colis�o.
        rb.isKinematic = true; // desativa a f�sica
    }

    private void OnTriggerEnter(Collider other) // caso entre em contato Trigger com um gameobject ir� fazer...
    {
        if(other.CompareTag("Point")) // caso o outro gameobject, tenha a tag Point, ir� entrar neste if...
        {
            other.gameObject.GetComponent<MeshRenderer>().material = changeMaterial; // aqui pegamos a refer�ncia do meshRenderer do gameObject com a tag Point.
            if(targetIndex < targets.Length)// aqui verificamos se o index atual � menor que o tamanho do array, para evitar problemas de out of bounds
            {
                targetIndex++; // incrementar mais um para o contador.
            }
        }
    }

}

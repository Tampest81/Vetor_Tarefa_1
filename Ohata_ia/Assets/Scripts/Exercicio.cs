using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exercicio : MonoBehaviour
{

    //Atribuição das variáveis

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
        // Verifica se a bool para multiplos alvos está ativada ou não
        if (!multipleTargets) // se não for ativo a bool para vários alvos irá entrar neste if
        {
            if (Vector3.Distance(this.transform.position, point.position) >= .05f) // verifica se a posição do alvo é maior que 0.5
            {
                MoveObject(point); // se for maior, irá chamar esse método, que recebe como parâmetro o transform do alvo
            }
            else
            {
                DisablePhysics(); // caso a distancia for menor que 0.5, irá chamar esse método que irá desativar a física do script RB
            }
        }
        // Verifica se o contador está menor que o tamanho do array pra não dar erro.
        else if (targetIndex < targets.Length) // dentro deste if, apenas irá rodar, caso seja multiplos alvos
        {
            // assim como no alvo único, verifica a distancia do gameobject e do alvo.
            if (Vector3.Distance(this.transform.position, MoveToTargets(targetIndex).position) >= .2f)
            {
                // caso entre neste método, irá chamar o MoveObject que é responsável pela movimentação, e como parâmetro, recebe o transform do alvo atual (index)
                MoveObject(MoveToTargets(targetIndex));
            }
        }
        // Caso alcance = Contador < Tamanho do array, irá entrar nesse else
        else
        {
            //Caso chegue na ultima posição, ele irá em direção ao penúltimo ponto.
            if (Vector3.Distance(this.transform.position, MoveToTargets(targetIndex - 2).position) >= .05f)
            {
                MoveObject(MoveToTargets(targetIndex - 2)); // irá se mover até o penúltimo alvo.
            }
            else
            {
                DisablePhysics();
            }
        }

    }
    
    private Transform MoveToTargets(int actualTarget) // método para fazer referência ao index atual, devolvendo assim o transform do alvo atual (index)
    {
        return targets[actualTarget]; // faz o retorno.
    }
    private void MoveObject(Transform target) // método para fazer a movimentação do gameObject
    {
        Vector3 targetVector = (target.position - this.gameObject.transform.position).normalized; // targetVector, um vetor que sempre aponta para o target
        rb.AddForce(targetVector * force, ForceMode.Force); // AddForce, aplicamos a força no RB para que possamos fazer a movimentação até o target, esta força é baseado no force
    }
    private void DisablePhysics() // desativa a física quando se aproxima do alvo para evitar o gameObject ficar tremendo/orbitar em volta
    {
        playerCollider.enabled = false; // desativa o colisor, caso haja mais de um com o mesmo alvo, acaba evitando problemas de colisão.
        rb.isKinematic = true; // desativa a física
    }

    private void OnTriggerEnter(Collider other) // caso entre em contato Trigger com um gameobject irá fazer...
    {
        if(other.CompareTag("Point")) // caso o outro gameobject, tenha a tag Point, irá entrar neste if...
        {
            other.gameObject.GetComponent<MeshRenderer>().material = changeMaterial; // aqui pegamos a referência do meshRenderer do gameObject com a tag Point.
            if(targetIndex < targets.Length)// aqui verificamos se o index atual é menor que o tamanho do array, para evitar problemas de out of bounds
            {
                targetIndex++; // incrementar mais um para o contador.
            }
        }
    }

}

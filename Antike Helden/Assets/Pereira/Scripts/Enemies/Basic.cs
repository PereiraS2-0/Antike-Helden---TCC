using UnityEngine;

public class BasicEnemyMovement : MonoBehaviour
{
    // Velocidade com que o inimigo se move.
    // [SerializeField] permite ajustar este valor no Inspector do Unity.
    [SerializeField]
    private float moveSpeed = 2f; // Voc� pode mudar esse valor inicial

    // Update � chamado uma vez por frame
    void Update()
    {
        // 1. Define o ponto de destino (o centro da cena)
        Vector3 targetPosition = Vector3.zero; // Vector3.zero � o mesmo que new Vector3(0, 0, 0)

        // 2. Calcula a dire��o do inimigo para o centro.
        //    (Posi��o do Alvo - Posi��o Atual) resulta em um vetor apontando para o alvo.
        //    .normalized transforma esse vetor em um vetor de tamanho 1 (s� queremos a dire��o).
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        // 3. Move o inimigo na dire��o calculada.
        //    Multiplicamos a dire��o pela velocidade e por Time.deltaTime.
        //    Time.deltaTime faz o movimento ser consistente independente da taxa de quadros (FPS).
        //    Space.World garante que o movimento seja relativo ao mundo, n�o � rota��o do objeto.
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // --- Alternativa usando Vector3.MoveTowards (faz quase a mesma coisa de forma um pouco diferente) ---
        // float step = moveSpeed * Time.deltaTime; // Calcula o quanto mover neste frame
        // transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        // --- Fim da Alternativa ---
    }
}

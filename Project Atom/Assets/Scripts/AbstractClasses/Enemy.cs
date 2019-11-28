using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Rigidbody rigid;
    public Animator anim;
    public ParticleSystem hurtEffect;
    public float life;
    public bool die;
    public float speed;
    public SkinnedMeshRenderer enemyShape;
    public Material standarMaterial;
    public Material poisonedMaterial;

    [Header("DropItem")]
    public DropItemController dropController;
    

    public abstract void Move();
    public abstract void Atack();
    public abstract void Die();
    public void DropItem(string itemName)
    {
        bool aux = false;
        int aux2 = 0;
        if(dropController.itemsPull.Exists(gameObject => gameObject.name == itemName +"(Clone)"))
            while (!aux)
            {
                if (!dropController.itemsPull[aux2].activeSelf && dropController.itemsPull[aux2].name == itemName + "(Clone)")
                {
                    dropController.itemsPull[aux2].transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y+1, gameObject.transform.position.z);
                    dropController.itemsPull[aux2].SetActive(true);
                    dropController.itemsPull[aux2].GetComponent<Rigidbody>().AddForce(
                        Vector3.forward * Random.Range(-2f, 2f) + Vector3.right * Random.Range(-2f, 2f), ForceMode.Impulse);
                    aux = true;
                }
                else
                {
                    aux2++;
                    if (aux2 > dropController.itemsPull.Count -1)
                        aux = true;
                }
            }

    }
}

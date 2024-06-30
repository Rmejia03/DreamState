using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextMelee : MonoBehaviour
{
    public float duration = 1f;
    public float floatSpeed = 1f;

    private TextMeshProUGUI textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        if(textMeshPro == null)
        {
            Debug.LogError("Component not found" + gameObject.name);
        }

        Destroy(gameObject,duration);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        textMeshPro.alpha -= Time.deltaTime / duration;
        
    }

    public void SetDamage(int damage)
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = damage.ToString();
        }
        else
        {
            Debug.LogError("Component is null in SetDamage");
        }
    }
}

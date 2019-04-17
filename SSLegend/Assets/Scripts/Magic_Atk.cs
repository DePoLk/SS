using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic_Atk : MonoBehaviour {
    private float timer = 0f;
    private float m_speed = 5f;
    private float m_RotSpeed=450f;
    private Boss_Arubado BA;
    private float m_currLife = 0f;
    public float m_MaxLife = 3f;
    private Vector3 pos;
    private Vector3 CurrentPos;
        // Use this for initialization
    void Start () {
        BA = FindObjectOfType<Boss_Arubado>();
        CurrentPos = BA.MagicAtk_Range.transform.position;
        Debug.Log(CurrentPos);
    }
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer < 0.5f)
        {
            m_speed += 2 * Time.deltaTime;
            transform.position += transform.forward * 3f * m_speed * Time.deltaTime;
        }
        else if (timer >= 0.5&& timer <= 1f)
        {
            pos = (CurrentPos - this.transform.position).normalized;
            Debug.Log(pos);
            float a = Vector3.Angle(transform.forward, pos) / m_RotSpeed;
            if (a > 0.1f || a < -0.1f)
            transform.forward = Vector3.Slerp(transform.forward, pos, Time.deltaTime / a).normalized;
            else
            {
                m_speed += 2 * Time.deltaTime;
                transform.forward = Vector3.Slerp(transform.forward, pos, 1).normalized;
            }
            transform.position += transform.forward * m_speed * 4 * Time.deltaTime;
        } else if (timer>1.5) {
            transform.position += transform.forward * m_speed * 8 * Time.deltaTime;
        }
        m_currLife += Time.deltaTime;
        if (m_currLife > m_MaxLife)
        {
            // 超过生命周期爆炸
            Destroy(gameObject);
            // Destroy(Instantiate(m_Explosion, transform.position, Quaternion.identity), 1.2f);
        }
	}
}

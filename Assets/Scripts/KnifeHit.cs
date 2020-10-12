using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeHit : MonoBehaviour
{
    Rigidbody2D rb;
    Islemler islemlerErisim;
    AudioSource kaynak;
    public AudioClip knifeHitFail;
    public GameObject portakalEfekt;
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        islemlerErisim = GameObject.Find("İşlemler").GetComponent<Islemler>();
        kaynak = GetComponentInParent<AudioSource>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "alt" || other.gameObject.tag=="trap")
        {
            kaynak.PlayOneShot(knifeHitFail);            
            rb.velocity = new Vector2(0, -20.0f);
            islemlerErisim.gameOver = true;
        }
        if (other.gameObject.tag == "portakal")
        {            
            portakalEfekt.SetActive(true);  //Portakal efekti aktif
            islemlerErisim.portakalSayisi++;            
            PlayerPrefs.SetInt("portakal", islemlerErisim.portakalSayisi);  //portakalSayisi' nin değerini PlayerPrefs'e set ettik            
            Destroy(portakalEfekt, 2.0f);   //2 sn sonra efekti yok et
            Destroy(other.gameObject);                 //Portakalı yok et
        }
    }
}
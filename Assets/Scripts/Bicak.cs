using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bicak : MonoBehaviour
{
    public int hiz;
    Rigidbody2D rb;
    public bool hazir;      //Fırlatılabilecek hazır bıçak olup olmadığını kontrol etmek için
    Islemler erisim;   //Islemler koduna erişim için 
    BoxCollider2D col;
    AudioSource kaynak;
    public AudioClip knifeHit;
    public GameObject tahtaEfekt;    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hazir = true;
        erisim = GameObject.Find("İşlemler").GetComponent<Islemler>();
        col = GetComponent<BoxCollider2D>();
        kaynak = GetComponent<AudioSource>();
    }  
    void Update()
    {
        if (Input.GetMouseButton(0) && hazir)
            Firlatma();
    }    
    void Firlatma()
    {
        rb.velocity = new Vector2(0, hiz);
        hazir = false;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        erisim.dogruAtilanBicak++;     //BicakSpawn çalışıyorsa bıçak doğru atılmıştır.
        if (erisim.dogruAtilanBicak == erisim.hedef)  
        {
            erisim.KütükParcala();
        }
        else
        {
            kaynak.PlayOneShot(knifeHit);
            tahtaEfekt.SetActive(true); //Tahta efektini burada aktif ettik
            Destroy(tahtaEfekt, 2.0f);  //Efektin duration'ı 2 sn old. için burada 2sn sonra yok dedik
            rb.isKinematic = true;      // gravity, mass gibi özellikleri kapatmak için
            rb.velocity = new Vector2(0, 0);
            transform.SetParent(other.transform);   //Trunk, bıçağın parent'ı oluyor.Yani bıçak, trunk'ın child'ı.Bıçağın trunk' ile yapışık dönmesi için
            col.enabled = false;    //simulate'i kapatmak yerine burada boxCollider'ı inaktif ettik.
            erisim.BicakSpawn();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //UI elemanlarını kontrol edebilmek için bu kütüphaneyi dahil ettik
using UnityEngine.SceneManagement;   //Stage sahne yönetimi için
public class Islemler : MonoBehaviour
{
    public GameObject bicak, portakal, trap;
    public Transform spawnPoint;
    public bool gameOver;    
    public GameObject[] noktalar;
    public int stage;
    public int hedef, dogruAtilanBicak, toplamBicakSayisi;
    public Text bicakSayisiText, stageText;
    public GameObject ikon, ikonPaneli, oyunIciPaneli, gameOverPaneli;
    public GameObject[] ikonlar;
    int ikonIndex = 0;     //Kaçıncı ikonun rengini değiştireceğini belirlemek için
    public int portakalSayisi;
    int a, b;  //portakal ve trap için random değer üretilip bu değişkene atılacak
    public Sprite[] bossSprites;
    public GameObject bossObjesi;
    public int stageNo;
    public GameObject cylinder1, cylinder2, cylinder3;
    public AudioClip breakSes;
    AudioSource breakKaynak;
    public bool parcalanma;
    bool bossFight;
    void Start()
    {
        parcalanma = false;
        breakKaynak = GetComponent<AudioSource>();
        bossObjesi.SetActive(false);
        oyunIciPaneli.SetActive(true);
        gameOverPaneli.SetActive(false);        
        stage = PlayerPrefs.GetInt("asama", 1);     //0, default başlangıç değerimiz
        toplamBicakSayisi = PlayerPrefs.GetInt("toplam", 0);
        portakalSayisi = PlayerPrefs.GetInt("portakal");
        bicakSayisiText.text = toplamBicakSayisi.ToString();
        stageText.text = "Stage: " + stage.ToString();
        ObjeSpawn();
        CheckStage();
        IkonYarat();
        if (stageNo % 5 == 0)
        {
            BossStage();
            bossFight = true;
        }
        else
        {
            bossFight = false;
        }
    }
    void Update()
    {
        if (gameOver)
        {
            StartCoroutine("GameOverPaneliniGetir");
        }
        else
        {
            StartCoroutine("NextStage");
            GameObject.Find("portakalSayisi").GetComponent<TMPro.TextMeshProUGUI>().text = portakalSayisi.ToString();
        }      
    }      
    void BossStage()
    {        
        bossObjesi.SetActive(true);
        cylinder1.SetActive(false);
        cylinder2.SetActive(false);
        cylinder3.SetActive(false);
        int bossSpriteIndex = Random.Range(0, 4);
        if (bossSpriteIndex == 0)
        {
            bossObjesi.GetComponent<SpriteRenderer>().sprite = bossSprites[bossSpriteIndex];
            stageText.text = "Boss: Katil Portakal";
        }
        else if (bossSpriteIndex == 1)
        {
            bossObjesi.GetComponent<SpriteRenderer>().sprite = bossSprites[bossSpriteIndex];
            stageText.text = "Boss: Serseri Kivi";
        }
        else if (bossSpriteIndex == 2)
        {
            bossObjesi.GetComponent<SpriteRenderer>().sprite = bossSprites[bossSpriteIndex];
            stageText.text = "Boss: Mars Hayatı";
        }
        else if (bossSpriteIndex == 3)
        {
            bossObjesi.GetComponent<SpriteRenderer>().sprite = bossSprites[bossSpriteIndex];
            stageText.text = "Boss: Kelek Karpuz";
        }
    }
    public void GameOver()  //Bunun için 0 başvuru gösterebilir ama aslında restart butonunun OnClick() metodunda kullanıyoruz zaten bu yüzden public
    {
        PlayerPrefs.DeleteKey("asama");     //Key'i asama olan PlayerPrefs'leri temizle
        PlayerPrefs.DeleteKey("toplam");    //Key'i toplam olan PlayerPrefs'leri temizle
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);     //Ve sonra sahneyi yeniden yükle
    }
    IEnumerator GameOverPaneliniGetir()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        gameOverPaneli.SetActive(true);
        oyunIciPaneli.SetActive(false);
        GameObject.Find("Score").GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + toplamBicakSayisi.ToString();
        GameObject.Find("PortakalSayısı").GetComponent<TMPro.TextMeshProUGUI>().text = portakalSayisi.ToString();
    }
    void CheckStage()
    {
        stageNo = PlayerPrefs.GetInt("asama",1);
        if (stageNo <= 2)
        {
            hedef = Random.Range(3, 5);
        }
        else if (2 < stageNo && stageNo <= 10)
        {
            hedef = Random.Range(5, 7);
        }
        else
        {
            hedef = Random.Range(7, 9);
        }
    }
    void IkonYarat()
    {
        ikonlar = new GameObject[hedef];
        for (int i = 0; i < hedef; i++)
        {
            GameObject go = Instantiate(ikon, ikonPaneli.transform);
            go.transform.SetParent(ikonPaneli.transform);
            ikonlar[i] = go;
        }
    }
    public void KütükParcala()
    {
        if (!bossFight)
        {               
            GameObject[] bicakUstUclari = GameObject.FindGameObjectsWithTag("Uc");
            foreach (GameObject item in bicakUstUclari)                         //Kütük parçalanırken bıçaklar birbirine çarparsa gameover olmasın diye uçlarının boxCollider'larını inaktif yaptım
            {
                item.GetComponent<BoxCollider2D>().enabled = false;
            }
            parcalanma = true;
            ikonlar[hedef - 1].GetComponent<Image>().color = Color.clear;            
            breakKaynak.PlayOneShot(breakSes);
            Rigidbody2D rb1 = cylinder1.GetComponent<Rigidbody2D>();
            Rigidbody2D rb2 = cylinder2.GetComponent<Rigidbody2D>();
            Rigidbody2D rb3 = cylinder3.GetComponent<Rigidbody2D>();
            rb1.velocity = new Vector2(Random.Range(-20, 20), Random.Range(-20, 20));
            rb2.velocity = new Vector2(Random.Range(-20, 20), Random.Range(-20, 20));
            rb3.velocity = new Vector2(Random.Range(-20, 20), Random.Range(-20, 20));
        }
    }
    IEnumerator NextStage()     
    {
        float beklemeSuresi;
        if (bossFight)
            beklemeSuresi = 0f;
        else
            beklemeSuresi = 0.4f;
       
        if (dogruAtilanBicak == hedef)
        {
            yield return new WaitForSecondsRealtime(beklemeSuresi);
            stage++;
            PlayerPrefs.SetInt("asama", stage);  //asama, get ve set işlemleri için uydurduğumuz bir keyword sözcük             
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);            
        }
    }
    public void BicakSpawn()    //Public yaptık çünkü Bicak kodundan buraya eriştik
    {
        if (!gameOver && dogruAtilanBicak-1 != hedef)
        {
            GameObject go = Instantiate(bicak, spawnPoint.transform.position, Quaternion.identity);
            toplamBicakSayisi++;    //High score niyetine koyduk
            PlayerPrefs.SetInt("toplam", toplamBicakSayisi);
            bicakSayisiText.text = PlayerPrefs.GetInt("toplam").ToString();
            ikonlar[ikonIndex].GetComponent<Image>().color = Color.clear;    //Bıçak ikonunu yok etmek için
            ikonIndex++;   //Bir sonraki ikon numarasına geçsin diye
        }        
    } 
    void PortakalYarat()
    {
        a = Random.Range(0, 8);
        if (noktalar[a].gameObject.transform.childCount == 0)   //childCount, 0'a eşitse yani bu noktada obje yoksa çalışsın
        {
            GameObject portakalObjesi = Instantiate(portakal, noktalar[a].transform);
            portakalObjesi.transform.SetParent(noktalar[a].transform);
        }
        else
            PortakalYarat();    //Burayı recursive de yaptık. Artık o nokta doluysa boş yer bulana kadar arayıp mecburen portakal üretecek ;) 
    }
    void TrapYarat()
    {
        b = Random.Range(0, 8);
        if (noktalar[b].gameObject.transform.childCount == 0)   //Aynı mantıkla childCount, 0'a eşitse yani bu noktada obje yoksa çalışsın
        {
            GameObject trapObjesi = Instantiate(trap, noktalar[b].transform);
            trapObjesi.transform.SetParent(noktalar[b].transform);
            if (b == 0)
                trapObjesi.transform.Rotate(0, 0, -190);
            else if (b == 1)
                trapObjesi.transform.Rotate(0, 0, 170);
            else if (b == 2)
                trapObjesi.transform.Rotate(0, 0, 160);
            else if (b == 3)
                trapObjesi.transform.Rotate(0, 0, 160);
            else if (b == 4)
                trapObjesi.transform.Rotate(0, 0, 155);
            else if (b == 5)
                trapObjesi.transform.Rotate(0, 0, 145);
            else if (b == 6)
                trapObjesi.transform.Rotate(0, 0, 150);
            else if (b == 7)
                trapObjesi.transform.Rotate(0, 0, 148);
            else
                TrapYarat();    //Burası da recursive olduğu için boş yer bulana kadar kendini yineleyecek
        }
    }
    void ObjeSpawn()    //public yapmadık çünkü zaten fonksiyonumuz işlemler kodunun içinde çalışacak
    {
        if (stage == 1)     //Stage 1'de sadece portakal olacak
        {
            a = Random.Range(0, 8);
            GameObject portakalObjesi = Instantiate(portakal, noktalar[a].transform.position, Quaternion.identity);
            portakalObjesi.transform.SetParent(noktalar[a].transform);
        }
        else    //Diğer durumlarda hem portakal hem trap yaratılacak
        {
            int toplamObjeSayisi = Random.Range(1, 4);
            for(int i = 0; i <= toplamObjeSayisi; i++)
            {
                float randomDeger = Random.Range(0, 10);
                if (randomDeger <= 5)
                    PortakalYarat();
                else
                    TrapYarat();
            }
        }
        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum GameState{
    SEQUENCIA,
    RESPONDER,
    NOVASEQUENCIA,
    ERRO
}

public class GameController : MonoBehaviour
{
    public GameState gameState;

    /*
     * statusFigura simula a ideia de "Aceso" e "Apagado" dos botões. Lembrando que o jogo é feito com sobreposição de figuras
     * statusFigura[0] = Apagado (Figura Transparente)
     * statusFigura[1] = Aceso (Figura não Transparentes)
     */
    public Color[] statusFigura;

    public Image[] button;
    public GameObject startButton;

    public Text rodadaTxt;
    public Text quantidadeNotasTxt;

    public List<int> listaFiguras; // Sequencia de figuras dos botões
    public int idResposta; // Saber qual a resposta que eu to dando para comparar com a figura que saiu;
    public int qtdFiguras; // Quantidade de figuras de cada turno
    public int rodada; 

    private AudioSource fonteAudio; 
    public AudioClip[] sons;


    void Start()
    {
        fonteAudio = GetComponent<AudioSource>();
        novaRodada();
    }

    public void StartGame(){
        StartCoroutine("sequencia", qtdFiguras+rodada);
    }

    public void novaRodada()
    {
        foreach (Image img in button)
        {
            img.color = statusFigura[0];
        }

        startButton.SetActive(true);
        rodadaTxt.text = "Rodada: " + (rodada + 1).ToString();
        quantidadeNotasTxt.text = "Sequencia: " + (qtdFiguras + rodada).ToString();
    }
    /*
     * MEMORIZAR SEQUENCIA DADA INICIALMENTE E ACRESCENTAR 1 NAS RODADAS SEGUINTES
     */
    IEnumerator sequencia(int qtd){
        startButton.SetActive(false);

        if(qtd == 3)
        {
            for (int r = qtd; r > 0; r--)
            {
                yield return new WaitForSeconds(0.5f);//Esperar meio segundo e vai executar o comando a seguir
                int i = Random.Range(0, button.Length);
                button[i].color = statusFigura[1]; 
                fonteAudio.PlayOneShot(sons[i]);

                listaFiguras.Add(i);//memoriza a sequencia para comparar  

                yield return new WaitForSeconds(0.5f);
                button[i].color = statusFigura[0];
            }
        }
        else
        {
            
            int i = Random.Range(0, button.Length);
            listaFiguras.Add(i);//memoriza a sequencia para comparar 
            for (int a = 0; a <qtd; a++)
            {
                int j = listaFiguras[a];
                yield return new WaitForSeconds(0.5f);
                button[j].color = statusFigura[1];
                fonteAudio.PlayOneShot(sons[j]);
                yield return new WaitForSeconds(0.5f);
                button[j].color = statusFigura[0];
            }
        }

        gameState = GameState.RESPONDER;
        idResposta = 0;
    }

    IEnumerator responder(int idBotao){
        button[idBotao].color = statusFigura[1];
        
        
        if(listaFiguras[idResposta] == idBotao)
        {
            fonteAudio.PlayOneShot(sons[idBotao]);
        }
        else
        {
            gameState = GameState.ERRO;
            StartCoroutine("GameOver");
        }
        idResposta++;
        if(idResposta == listaFiguras.Count)
        {
            gameState = GameState.NOVASEQUENCIA;
            rodada++;
            yield return new WaitForSeconds(1);
            novaRodada();
        }

        yield return new WaitForSeconds(0.3f);
        button[idBotao].color = statusFigura[0];
    }

    IEnumerator GameOver()
    {
        rodada = 0;
        fonteAudio.PlayOneShot(sons[4]);
        yield return new WaitForSeconds(1);

        for (int aux = 3; aux > 0; aux--)
        {
            foreach (Image img in button)
            {
                img.color = statusFigura[1];
            }

            yield return new WaitForSeconds(0.3f);

            foreach (Image img in button)
            {
                img.color = statusFigura[0];
            }
            yield return new WaitForSeconds(0.3f);
        }

        int idB = 0;
        for (int i = 12; i > 0; i--)
        {
            button[idB].color = statusFigura[1];
            yield return new WaitForSeconds(0.1f);
            button[idB].color = statusFigura[0];
            yield return new WaitForSeconds(0.1f);
            idB++;
            if(idB == 4)
            {
                idB = 0;
            }
        }
        listaFiguras.Clear();
        gameState = GameState.NOVASEQUENCIA;
   
        novaRodada();
    }

}

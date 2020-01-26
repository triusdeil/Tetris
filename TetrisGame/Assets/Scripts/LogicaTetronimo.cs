using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LogicaTetronimo : MonoBehaviour
{
    private float TiempoAnterior;
    public float TiempoCaida = 0.8f;

    public static int alto = 20;
    public static int ancho = 10;

    public Vector3 puntoRotation;

    public static int Puntaje = 0;
    public static int NivelDificultad = 0;

    private static Transform[,]grid=new Transform[ancho,alto];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //teclas para movimiento
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position  += new Vector3 (-1,0,0);
            if(!Limites())
            {
                transform.position  -= new Vector3 (-1,0,0);
            }
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position  += new Vector3 (1,0,0);
            if(!Limites())
            {
                transform.position  -= new Vector3 (1,0,0);
            }
        }
        //tiempo de caida
        if(Time.time - TiempoAnterior >(Input.GetKey(KeyCode.DownArrow)?TiempoCaida/20: TiempoCaida))
        {
            transform.position += new Vector3(0,-1,0);
            if(!Limites())
            {
                transform.position  -= new Vector3 (0,-1,0);
                AñadirGrid();
                Revisarlineas();
                //para que no se mueva al llegar al final
                this.enabled = false;
                FindObjectOfType<LogicaGenerador>().NuevoTetrominos();
            }
            TiempoAnterior = Time.time;
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(puntoRotation),new Vector3(0,0,1),-90);
            //salir del limite regresar
            if(!Limites())
            {
                transform.RotateAround(transform.TransformPoint(puntoRotation),new Vector3(0,0,1),90);
            }
        }
        AumentarNivel();
        AumentarDificultad();
    }
    //Limites para que no se vaya del fondo
    bool Limites()
    {
        foreach(Transform hijo in transform)
        {
            int enteroX = Mathf.RoundToInt(hijo.transform.position.x);
            int enteroY = Mathf.RoundToInt(hijo.transform.position.y);

            if(enteroX <0 || enteroX >= ancho||enteroY<0||enteroY>=alto)
            {
                return false;
            }
              //si en el grid choca con otra posicion se detenga
        if(grid[enteroX,enteroY]!=null)
        {
            return false;
        }
        }
      
        return true;
    }

    void AñadirGrid()
    {
        foreach(Transform hijo in transform)
        {
            int enteroX = Mathf.RoundToInt(hijo.transform.position.x);
            int enteroY = Mathf.RoundToInt(hijo.transform.position.y);

            grid[enteroX,enteroY] = hijo;
            if(enteroY >= 19)
            {
                Puntaje = 0;
                NivelDificultad = 0;
                TiempoCaida = 0.8f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
    //Metodo para revisar y borrar  
    void Revisarlineas()
    {
        for(int i = alto -1;i >= 0; i--)
        {
            if(TieneLinea(i))
            {
                BorrarLinea(i);
                BajarLinea(i);
            }
        }
    }

    bool TieneLinea(int i)
    {
        for(int j = 0;j < ancho; j++)
        {
            if(grid[j,i]== null)
            {
                return false;
            }
        }
        Puntaje+=100;
        Debug.Log(Puntaje);
        return true;
    }

    void BorrarLinea(int i)
    {
        for(int j = 0;j < ancho; j++)
        {
            Destroy(grid[j,i].gameObject);
            grid[j,i]= null;
        }
    }
    void BajarLinea(int i)
    {
        for(int y = i;y < alto; y++)
        {
            for(int j = 0;j < ancho; j++)
            {
                if(grid[j,y]!=null)
                {
                    grid[j,y-1] = grid[j,y];
                    grid[j,y] = null;
                    grid[j,y-1].transform.position-=new Vector3(0,1,0);
                }
            }
        }
    }

    void AumentarNivel()
    {
        switch(Puntaje)
        {
            case 200:
                NivelDificultad = 1;
                break;
            case 400:
                NivelDificultad = 2;
                break;
            case 600:
                NivelDificultad = 3;
                break;
            case 800:
                NivelDificultad = 4;
                break;
            case 1000:
                NivelDificultad = 5;
                break;
            case 1200:
                NivelDificultad = 6;
                break;
            case 1400:
                NivelDificultad = 7;
                break;
            case 1600:
                NivelDificultad = 8;
                break;
            case 1800:
                NivelDificultad = 9;
                break;
            case 2000:
                NivelDificultad = 10;
                break;
        }
    }
    void AumentarDificultad()
    {
        switch(NivelDificultad)
        {
            case 1:
                TiempoCaida = 0.7f;
                break;
            case 2:
                TiempoCaida = 0.6f;
                break;
            case 3:
                TiempoCaida = 0.5f;
                break;
            case 4:
                TiempoCaida = 0.4f;
                break;
            case 5:
                TiempoCaida = 0.3f;
                break;
            case 6:
                TiempoCaida = 0.25f;
                break;
            case 7:
                TiempoCaida = 0.2f;
                break;
            case 8:
                TiempoCaida = 0.15f;
                break;
            case 9:
                TiempoCaida = 0.1f;
                break;
            case 10:
                TiempoCaida = 0.05f;
                break;
        }
    }
}

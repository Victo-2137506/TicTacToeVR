using UnityEngine;
using TMPro;

/// <summary>
/// Ga
/// </summary>
public class GameManager : MonoBehaviour
{
    // Instance pour acceder au Gamemanager
    public static GameManager instance { get; private set; }

    public char joueurActuel = 'X';
    public bool partieEnCours = true;
    private char[] cases = new char[9];

    public TextMeshProUGUI texteTour;

    public GameObject objetX;
    public GameObject objetO;

    void Awake()
    {
        // Si une autre instance existe déjà, on détruit celle-ci
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        InitialiserPartie();
    }

    /// <summary>
    /// Méthode pour jouer les symboles
    /// </summary>
    /// <param name="indexCase">Case de 1-9</param>
    /// <param name="symbole">X ou O</param>
    /// <returns></returns>
    public bool JouerSymbole(int indexCase, char symbole)
    {
        // Si l'index est invalide, on ne joue pas
        if (indexCase < 0 || indexCase >= cases.Length) return false;

        // Enregistre le symbole dans la grille logique
        cases[indexCase] = symbole;

        if (VerifierVictoire(symbole))
        {
            PartieTerminee($"{symbole} a gagné !");

            // Déclencher l'animation du gagnant (Ne marche pas)
            GameObject cible = (symbole == 'X') ? objetX : objetO;
            if (cible != null)
            {
                Animator anim = cible.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.SetTrigger("gagner");
                }
            }

            return true;
        }

        if (VerifierNul())
        {
            PartieTerminee("Match nul !");
            return true;
        }

        // Changer de joueur et met à jour le texte
        joueurActuel = joueurActuel == 'X' ? 'O' : 'X';
        if (texteTour != null)
            texteTour.text = $"Tour de {joueurActuel}";

        return true;
    }

    /// <summary>
    /// Méthode pour initialiser la partie.
    /// </summary>
    public void InitialiserPartie()
    {
        for (int i = 0; i < cases.Length; i++)
            cases[i] = '\0';

        joueurActuel = 'X';
        partieEnCours = true;

        if (texteTour != null)
            texteTour.text = "Tour de X";
    }

    /// <summary>
    /// Méthode pour termine la partie.
    /// </summary>
    /// <param name="message"></param>
    void PartieTerminee(string message)
    {
        partieEnCours = false;

        if (texteTour != null)
            texteTour.text = message; // ex: "X/O a gagné !" ou "Match nul !"

        Debug.Log(message);
    }

    /// <summary>
    /// Méthode pour vérifier les conditions de victoire.
    /// </summary>
    /// <param name="symbole"></param>
    /// <returns></returns>
    bool VerifierVictoire(char symbole)
    {
        int[][] combinaisons = new int[][]
        {
            // Ligne verticale
            new int[] {0, 1, 2}, 
            new int[] {3, 4, 5}, 
            new int[] {6, 7, 8},
            // Ligne horizontale
            new int[] {0, 3, 6}, 
            new int[] {1, 4, 7}, 
            new int[] {2, 5, 8},
            // Ligne diagonale
            new int[] {0, 4, 8}, 
            new int[] {2, 4, 6}
        };

        // Vérifie chaque combinaison
        foreach (var combo in combinaisons)
        {
            if (cases[combo[0]] == symbole && cases[combo[1]] == symbole && cases[combo[2]] == symbole)
                return true; // Ligne générer par ChatGPT
        }
        return false;
    }

    /// <summary>
    /// Méthode pour vérifier si le partie est nul.
    /// </summary>
    /// <returns></returns>
    bool VerifierNul()
    {
        foreach (char c in cases)
            if (c == '\0') return false;
        return true;
    }
}

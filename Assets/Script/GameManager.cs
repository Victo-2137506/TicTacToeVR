using UnityEngine;
using TMPro;

/// <summary>
/// 
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public char joueurActuel = 'X';
    public bool partieEnCours = true;
    private char[] cases = new char[9];

    public TextMeshProUGUI texteTour;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        InitialiserPartie();
    }

    public bool JouerSymbole(int indexCase, char symbole)
    {
        if (!partieEnCours) return false;
        if (indexCase < 0 || indexCase >= cases.Length) return false;
        if (cases[indexCase] != '\0') return false;

        cases[indexCase] = symbole;

        if (VerifierVictoire(symbole))
        {
            PartieTerminee($"{symbole} a gagné !");
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

    public void InitialiserPartie()
    {
        for (int i = 0; i < cases.Length; i++)
            cases[i] = '\0';

        joueurActuel = 'X';
        partieEnCours = true;

        // Réinitialiser le texte
        if (texteTour != null)
            texteTour.text = "Tour de X";
    }

    void PartieTerminee(string message)
    {
        partieEnCours = false;

        if (texteTour != null)
            texteTour.text = message; // ex: "X/O a gagné !" ou "Match nul !"

        Debug.Log(message);
    }

    bool VerifierVictoire(char symbole)
    {
        int[][] combinaisons = new int[][]
        {
            new int[] {0, 1, 2}, new int[] {3, 4, 5}, new int[] {6, 7, 8},
            new int[] {0, 3, 6}, new int[] {1, 4, 7}, new int[] {2, 5, 8},
            new int[] {0, 4, 8}, new int[] {2, 4, 6}
        };

        foreach (var combo in combinaisons)
        {
            if (cases[combo[0]] == symbole && cases[combo[1]] == symbole && cases[combo[2]] == symbole)
                return true;
        }
        return false;
    }

    bool VerifierNul()
    {
        foreach (char c in cases)
            if (c == '\0') return false;
        return true;
    }
}
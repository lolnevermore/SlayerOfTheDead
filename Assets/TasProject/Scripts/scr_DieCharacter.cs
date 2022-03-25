using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class scr_DieCharacter : MonoBehaviour
{
    public static int playerHP = 500;
    public TextMeshProUGUI playerHPText;
    public int damageAmount = 20;





    void Update()
    {

        playerHPText.text = "+" + playerHP.ToString();

    }
    public void DealDamage(int damageAmount)
    {
        playerHP -= damageAmount;

        Debug.Log(playerHP);

        if (playerHP <= 0)
        {
            SceneManager.LoadScene("TasScene");
            print("player being hit");

        }
        //print("being called");
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Enemy")
        {
            print("Collided with the player.");

            scr_Target enemy = hit.gameObject.GetComponent<scr_Target>();

            DealDamage(enemy.damageAmount);
        }

    }
}







using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Register : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private string registerEndpoint ;

    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private Button registerButton;
    [SerializeField] private TMP_InputField firstnameInputField;
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField repassInputField;
    [SerializeField] private TMP_InputField phoneInputField;
    [SerializeField] private TMP_Dropdown gouvDropdown;
    [SerializeField] private TMP_Dropdown munDropdown;


    string firstname;
    string Email;
    string password;
    string repass;
    string phone;
    public void OnRegisterClick()
    {
        alertText.text = "Loading...";
        ActivateButtons(false);

        StartCoroutine(TryRegister());
    }

    private IEnumerator TryRegister()
    {

        firstname = firstnameInputField.text;
        Email = emailInputField.text;
        password = passwordInputField.text;
        repass = repassInputField.text;
        phone = phoneInputField.text;

        print(repass);
        print(password);

        //string gouv = gouvDropdown.value.ToString();
        //string mun = munDropdown.value.ToString();

        if (repass != password)
        {
            print("here");
            alertText.text = "password doesn't match";
            yield break;
        }

        WWWForm form = new WWWForm();

        form.AddField("Email", Email);
        form.AddField("firstname", firstname);
        form.AddField("password", password);
        form.AddField("phone", phone);


        UnityWebRequest request = UnityWebRequest.Post(registerEndpoint, form);
        var handler = request.SendWebRequest();

        float startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;

            if (startTime > 10.0f)
            {
                break;
            }

            yield return null;
        }


        if (request.result == UnityWebRequest.Result.Success)
        {  
            LoginData.LoginResponse response = JsonUtility.FromJson<LoginData.LoginResponse>(request.downloadHandler.text);
            if (response.code == 0)
            {
                ActivateButtons(false);
                alertText.text = "Account has been created!";

                PlayerPrefs.SetString("isAuthenticated", "true");
                PlayerPrefs.SetString("token_user", response.data.accessToken);
 
                UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            }
            else
            {
                switch (response.code)
                {
                    case 1:
                        alertText.text = "All input is required";
                        ActivateButtons(true);
                        break;
                    case 2:
                        alertText.text = "Account already exist. Please Login";
                        ActivateButtons(true);
                        break;
                    case 3:
                        alertText.text = "Password is unsafe";
                        ActivateButtons(true);
                        break;
                    default:
                        alertText.text = "Corruption detected";
                        ActivateButtons(true);
                        break;

                }
            }
        }
        else
        {
            alertText.text = "Error connecting to the server...";
        }

        ActivateButtons(true);

        yield return null;
    }


    private void ActivateButtons(bool toggle)
    {
        registerButton.interactable = toggle;
    }

    public void Back()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }


}

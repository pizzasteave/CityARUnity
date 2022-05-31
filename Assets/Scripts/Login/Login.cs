using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Login : MonoBehaviour
{
    [SerializeField] private string loginEndpoint = "http://127.0.0.1:8080/api/login";


    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private Button loginButton;
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;



    public void OnLoginClick()
    {
        alertText.text = "Loading...";
        ActivateButtons(false);

        StartCoroutine(TryLogin());
    }

    private IEnumerator TryLogin()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        UnityWebRequest request = UnityWebRequest.Post(loginEndpoint, form);
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
                alertText.text = "Welcome";

                PlayerPrefs.SetString("isAuthenticated", "true");
                PlayerPrefs.SetString("id_user", response.data._id);
                PlayerPrefs.SetString("token_user", response.data.accessToken);
                print(response.data.accessToken);

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
                        alertText.text = "Password incorrect";
                        ActivateButtons(true);
                        break;
                    case 3:
                        alertText.text = "Account not existed , please sign up !!";
                        ActivateButtons(true);
                        break;
                    default:
                        alertText.text = "Corruption detected";
                        ActivateButtons(false);
                        break;
                }
            }
        }
        else
        {
            alertText.text = "Error connecting to the server";
            ActivateButtons(true);
        }

        yield return null;
    }

   
    private void ActivateButtons(bool toggle)
    {
        loginButton.interactable = toggle;
    }

    public void Back()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Welcome2");
    }
}

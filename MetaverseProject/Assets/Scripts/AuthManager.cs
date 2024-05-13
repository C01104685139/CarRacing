using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class AuthManager : MonoBehaviour
{
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    public TMP_InputField email;
    public TMP_InputField password;
    public GameObject isFailedSignUpPanel;
    public GameObject isFailedSignInPanel;

    private string displayName;
    private string emailAddress;

    private bool isSignedIn;
    private bool isFailedSignUp;
    private bool isFailedSignIn;

    // Start is called before the first frame update
    void Start()
    {
        isSignedIn = false;
        isFailedSignUp = false;
        isFailedSignIn = false;
        InitializeFirebase();
    }

    void Update()
    {
        // �α��� ���� �� ���� ȭ������ �� ��ȯ
        if (isSignedIn)
        {
            SceneManager.LoadScene("SampleScene");
        }

        // ȸ������ ���� �� �˾�â ����
        if (isFailedSignUp)
        {
            isFailedSignUpPanel.SetActive(true);
            isFailedSignUp = false;
        }

        // �α��� ���� �� �˾�â ����
        if (isFailedSignIn)
        {
            isFailedSignInPanel.SetActive(true);
            isFailedSignIn = false;
        }

    }

    // ���� ���� �� �ڵ� �α׾ƿ�
    void OnApplicationQuit()
    {
        if (auth != null && auth.CurrentUser != null)
        {
            auth.SignOut();
        }
    }

    // ȸ������
    public void signUp()
    {
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);

                // �̹� ��ϵ� ����ڷ� ȸ������ �õ� �� �˾�â ����
                isFailedSignUp = true;

                return;
            }

            // Firebase user has been created.
            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });
    }

    // �α���
    public void signIn()
    {
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);

                // Ʋ�� ��й�ȣ or ��ϵ��� ���� ����ڷ� �α��� �õ� �� �˾�â ����
                isFailedSignIn = true;

                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            isSignedIn = true;
        });
    }

    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null
                && auth.CurrentUser.IsValid();
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                displayName = user.DisplayName ?? "";
                emailAddress = user.Email ?? "";
            }
        }
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }
}

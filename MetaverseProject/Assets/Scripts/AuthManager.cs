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
        // 로그인 성공 시 대기실 화면으로 씬 전환
        if (isSignedIn)
        {
            SceneManager.LoadScene("SampleScene");
        }

        // 회원가입 실패 시 팝업창 생성
        if (isFailedSignUp)
        {
            isFailedSignUpPanel.SetActive(true);
            isFailedSignUp = false;
        }

        // 로그인 실패 시 팝업창 생성
        if (isFailedSignIn)
        {
            isFailedSignInPanel.SetActive(true);
            isFailedSignIn = false;
        }

    }

    // 게임 종료 시 자동 로그아웃
    void OnApplicationQuit()
    {
        if (auth != null && auth.CurrentUser != null)
        {
            auth.SignOut();
        }
    }

    // 회원가입
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

                // 이미 등록된 사용자로 회원가입 시도 시 팝업창 생성
                isFailedSignUp = true;

                return;
            }

            // Firebase user has been created.
            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });
    }

    // 로그인
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

                // 틀린 비밀번호 or 등록되지 않은 사용자로 로그인 시도 시 팝업창 생성
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

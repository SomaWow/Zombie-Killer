using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Menu
{
    public class MenuSetting : MenuAbs
    {
        private CanvasGroup canvasGroup;
        private Button closeButton;
        private Toggle musicToggle;
        private Toggle sfxToggle;
        private InputField usernameInput;


        private void Awake()
        {
            InitComponent();

            canvasGroup.alpha = 0;
            this.gameObject.SetActive(false);
        }

        private void InitComponent()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            closeButton = Find<Button>("closeButton");
            closeButton.onClick.AddListener(CloseClick);

            musicToggle = Find<Toggle>("musicToggle");
            musicToggle.onValueChanged.AddListener(MusicClick);

            sfxToggle = Find<Toggle>("sfxToggle");
            sfxToggle.onValueChanged.AddListener(SoundClick);

            usernameInput = Find<InputField>("usernameInputField");
            usernameInput.onValueChanged.AddListener(InputUsername);
        }

        public void Refresh()
        {
            musicToggle.isOn = Profile.musicEnable;
            sfxToggle.isOn = Profile.soundEnable;
            usernameInput.text = Profile.username;
        }

        public override void BackClick()
        {
            CloseClick();
        }

        private void CloseClick()
        {
            Hide();
            // 声音
            AudioManager.INSTANCE.Play("UI/button2");
        }

        public override void Show()
        {
            Debug.Log("+++ MenuSetting.Show()");
            this.gameObject.SetActive(true);
            canvasGroup.DOFade(1, 0.2f);
            this.Refresh();
        }
        public override void Hide()
        {
            Debug.Log("+++ MenuSetting.Hide()");
            canvasGroup.DOFade(0, 0.2f).OnComplete(HideEnd);
        }
        private void HideEnd()
        {
            Menu.INSTANCE.mTopBar.greetBoardText.text = "Hi! " + Profile.username;
            this.gameObject.SetActive(false);
        }

        public void MusicClick(bool isMusic)
        {
            Profile.musicEnable = isMusic;
            Profile.UpdateSettings();
            if(isMusic)
            {
                AudioManager.INSTANCE.PlayBG("BG/menu");
            }
            else
            {
                AudioManager.INSTANCE.StopBG();
            }
            AudioManager.INSTANCE.Play("UI/tab");
        }

        public void SoundClick(bool isSound)
        {
            Profile.soundEnable = isSound;
            Profile.UpdateSettings();
            AudioManager.INSTANCE.Play("UI/tab");
        }

        public void InputUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                Profile.username = "Nameless";
            }
            else
            {
                Profile.username = username;
            }
            Profile.UpdateSettings();
        }
    }
}
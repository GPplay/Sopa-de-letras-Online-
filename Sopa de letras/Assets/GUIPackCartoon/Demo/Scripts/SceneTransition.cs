using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ricimi
{
    public class SceneTransition : MonoBehaviour
    {
        public string scene = "";
        public float duration = 1.0f;
        public Color color = Color.black;

        public void PerformTransition()
        {
            // Eliminar referencia a Transition
            StartCoroutine(LoadLevel(scene));
        }

        public void ExitGame()
        {
            Time.timeScale = 1.0f;
            StartCoroutine(ExitAfterTransition());
        }

        private IEnumerator ExitAfterTransition()
        {
            yield return new WaitForSeconds(duration);
            QuitApplication();
        }

        private IEnumerator LoadLevel(string sceneName)
        {
            // Transición básica de fade
            float elapsed = 0f;
            CanvasGroup fade = CreateFadeCanvas();

            while (elapsed < duration)
            {
                fade.alpha = Mathf.Clamp01(elapsed / duration);
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
        }

        private CanvasGroup CreateFadeCanvas()
        {
            GameObject fadeObject = new GameObject("FadeCanvas");
            Canvas canvas = fadeObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 9999;

            CanvasGroup group = fadeObject.AddComponent<CanvasGroup>();
            UnityEngine.UI.Image image = fadeObject.AddComponent<UnityEngine.UI.Image>();
            image.color = color;

            RectTransform rt = fadeObject.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;

            return group;
        }

        private void QuitApplication()
        {
            #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
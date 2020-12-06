using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class FadeControlUI : MonoBehaviour
    {

        public GameObject fadePanel;
        public CanvasGroup m_uiElement;

        private float m_timeStartedLerping;
        private float m_timeSinceStarted;
        private float m_percentageComplete;
        private float m_lerpTime;
        private float m_start;
        private float m_end;

        private bool m_bStart;

        //
        public void FadeIn()
        {
            FadeUI(m_uiElement, 0.0f, 1.0f);
        }

        //
        public void FadeOut()
        {
            FadeUI(m_uiElement, 1.0f, 0.0f);
        }

        //
        public void FadeUI(CanvasGroup cg, float start, float end, float lerpTime = 1.0f)
        {
            fadePanel.SetActive(true);

            m_bStart = true;
            m_uiElement.alpha = start;

            m_timeStartedLerping = Time.time;
            m_timeSinceStarted = Time.time - m_timeStartedLerping;
            m_percentageComplete = m_timeStartedLerping / lerpTime;
            m_start = start;
            m_end = end;
            m_lerpTime = lerpTime;

            Invoke("Completed", lerpTime);
        }

        //
        void Start()
        {
            m_timeStartedLerping = 0;
            m_timeSinceStarted = 0;
            m_percentageComplete = 0;
            m_start = 0;
            m_end = 1;
            m_lerpTime = 0;
        }

        //
        void FixedUpdate()
        {
            if (m_bStart)
            {
                m_timeSinceStarted = Time.time - m_timeStartedLerping;
                m_percentageComplete = m_timeSinceStarted / m_lerpTime;

                float currentValue = Mathf.Lerp(m_start, m_end, m_percentageComplete);

                m_uiElement.alpha = currentValue;
            }

        }

        //
        private void Completed()
        {
            m_bStart = false;
        }
    }
}


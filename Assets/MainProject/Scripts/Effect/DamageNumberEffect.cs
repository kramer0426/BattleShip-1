using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class DamageNumberEffect : MonoBehaviour
    {
        public NumberControlUI numberControl;

        private Vector3 toPos = new Vector3(0.0f, 0.0f, 0.0f);
        private Color numberColor;

        //
        public void CreateFx(int damage, bool bCritical)
        {
            if (bCritical)
            {
                numberControl.SetColor(Color.yellow);
                numberColor = Color.yellow;
            }
            else
            {
                numberControl.SetColor(Color.white);
                numberColor = Color.white;
            }
            numberControl.SetNumner(damage);

            toPos = transform.position;
            toPos.y += 0.4f;
            LeanTween.move(this.gameObject, toPos, 0.3f / BattleControl.Instance.battleTimeScale_).setEase(LeanTweenType.easeOutQuad).setOnComplete(
                            () =>
                            {
                                LeanTween.value(this.gameObject, 1.0f, 0.0f, 0.3f / BattleControl.Instance.battleTimeScale_).setEase(LeanTweenType.linear).setOnUpdate(value =>
                                {
                                    numberColor.a = value;
                                    numberControl.SetColor(numberColor);

                                });
                            });

            Invoke("DestroyEffect", 2.0f / BattleControl.Instance.battleTimeScale_);
        }

        //
        public void DestroyEffect()
        {
            Destroy(this.gameObject);
        }
    }
}


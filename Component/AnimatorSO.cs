using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Component/AnimatorSO")]
    public class AnimatorSO : ComponentSO
    {
        public Animator animator;

        public AnimatorController animatorController;

        public Avatar avatar;

        public override void AddComponent(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out animator)) return;
            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
            animator.avatar = avatar;
        }

        public bool CompareAnimatorController(AnimatorController animatorController)
        {
            return animatorController == this.animatorController;
        }

        public void SetAnimatorParameter(string name)
        {
            animator.SetTrigger(name);
        }

        public void SetAnimatorParameter(string name, bool key)
        {
            animator.SetBool(name, key);
        }

        public void SetAnimatorParameter(string name, float value)
        {
            animator.SetFloat(name, value);
        }

        public bool HaveParameter(string name)
        {
            foreach( var para  in animator.parameters )
            {
                if(para.name == name) return true;
            }
            return false;
        }
    }

}


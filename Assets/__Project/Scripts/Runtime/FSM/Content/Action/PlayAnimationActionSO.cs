using UnityEngine;

[CreateAssetMenu(fileName = "PlayAnimationAction", menuName = "RWM/FSM/Actions/PlayAnimation")]
public class PlayAnimationActionSO : FSMActionSO
{
    [SerializeField] private string animationName;

    public override void Execute(FSMCtrl ctrl)
    {
        if (ctrl.Animator)
        {
            ctrl.Animator.Play(animationName);
        }
    }
}

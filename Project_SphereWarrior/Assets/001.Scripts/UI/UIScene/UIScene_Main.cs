using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.AddressableAssets.Build.Layout.BuildLayout;

public class UIScene_Main : UIScene
{
    private Vector2 dragPointOne;
    private Sequence sequence;
    private int nowBundle;


    public override bool Init()
    {
        if(! base.Init()) return false;
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(Objects));

        BindEvent(GetImage((int)Images.Image_SwipePlace).gameObject, _dragCallback: BeginDrag, _type: Define.UIEventType.BeginDrag);
        BindEvent(GetImage((int)Images.Image_SwipePlace).gameObject, _dragCallback: Drag, _type: Define.UIEventType.Drag);
        BindEvent(GetButton((int)Buttons.Button_BallUpgrade).gameObject, OnClick_BallUpgrade);
        BindEvent(GetButton((int)Buttons.Button_Shop).gameObject, OnClick_Shop);
        BindEvent(GetButton((int)Buttons.Button_Skill).gameObject, OnClick_Skill);
        BindEvent(GetButton((int)Buttons.Button_Soul).gameObject, OnClick_Soul);
        BindEvent(GetButton((int)Buttons.Button_SquareUpgrade).gameObject, OnClick_SquareUpgrade);

        CloseAllBundle();
        Managers.UI.SceneUI = this;
        return true;
    }

    #region Swipe

    public override void RedrawUI()
    {
        GetText((int)Texts.Text_Gold).text = $"{Managers.Game.player.gold} Gold";
    }

    public void BeginDrag(PointerEventData _data)
    {
        dragPointOne = _data.position;
    }

    public void Drag(PointerEventData _data)
    {
        Managers.Input.SetSwipeDirection((_data.position - dragPointOne).normalized);
        Managers.Input.SetSwipeForce(Vector2.Distance(_data.position, dragPointOne));
        dragPointOne = _data.position;
    }
    #endregion
    #region Buttons
    private void OnClick_BallUpgrade()
    {
        RectTransform bundle = GetObject((int)Objects.Bundle_BallUpgrade).GetComponent<RectTransform>();

        if (nowBundle != (int)Objects.Bundle_BallUpgrade)
        {
            CloseAllBundle();
            OpenBundle(bundle);
            nowBundle = (int)Objects.Bundle_BallUpgrade;
        } 

        else
        {
            CloseBundle(bundle);
            nowBundle = -1;
        }
    }

    private void OnClick_Shop()
    {
        RectTransform bundle = GetObject((int)Objects.Bundle_Shop).GetComponent<RectTransform>();

        if (nowBundle != (int)Objects.Bundle_Shop)
        {
            CloseAllBundle();
            OpenBundle(bundle);
            nowBundle = (int)Objects.Bundle_Shop;
        }

        else
        {
            CloseBundle(bundle);
            nowBundle = -1;
        }
    }

    private void OnClick_Skill()
    {
        RectTransform bundle = GetObject((int)Objects.Bundle_Skill).GetComponent<RectTransform>();

        if (nowBundle != (int)Objects.Bundle_Skill)
        {
            CloseAllBundle();
            OpenBundle(bundle);
            nowBundle = (int)Objects.Bundle_Skill;
        }

        else
        {
            CloseBundle(bundle);
            nowBundle = -1;
        }
    }

    private void OnClick_Soul()
    {
        RectTransform bundle = GetObject((int)Objects.Bundle_Soul).GetComponent<RectTransform>();

        if (nowBundle != (int)Objects.Bundle_Soul)
        {
            CloseAllBundle();
            OpenBundle(bundle);
            nowBundle = (int)Objects.Bundle_Soul;
        }

        else
        {
            CloseBundle(bundle);
            nowBundle = -1;
        }
    }

    private void OnClick_SquareUpgrade()
    {
        RectTransform bundle = GetObject((int)Objects.Bundle_SquareUpgrade).GetComponent<RectTransform>();

        if (nowBundle != (int)Objects.Bundle_SquareUpgrade)
        {
            CloseAllBundle();
            OpenBundle(bundle);
            nowBundle = (int)Objects.Bundle_SquareUpgrade;
        }

        else
        {
            CloseBundle(bundle);
            nowBundle = -1;
        }
    }

    private void OpenBundle(RectTransform _bundle)
    {
        _bundle.position = GetObject((int)Objects.Trans_BundleStartTweeing).transform.position;
        _bundle.gameObject.SetActive(true);
        DOTween.Kill($"{nameof(CloseBundle)}");
        sequence = DOTween.Sequence().SetId($"{nameof(OpenBundle)}");
        sequence.Join(_bundle.DOMove(GetObject((int)Objects.Trans_BundleEndTweeing).transform.position, 1));
        sequence.AppendCallback(() => { sequence = null; });
        sequence.Play();
    }

    private void CloseBundle(RectTransform _bundle)
    {
        DOTween.Kill($"{nameof(OpenBundle)}");
        sequence = DOTween.Sequence().SetId($"{nameof(CloseBundle)}");
        sequence.Join(_bundle.DOMove(GetObject((int)Objects.Trans_BundleStartTweeing).transform.position, 1));
        sequence.AppendCallback(() => { _bundle.gameObject.SetActive(false); sequence = null; });
        sequence.Play();
    }

    private void CloseAllBundle()
    {
        if (sequence != null)
            sequence = null;
        GetObject((int)Objects.Bundle_BallUpgrade).SetActive(false);
        GetObject((int)Objects.Bundle_Shop).SetActive(false);
        GetObject((int)Objects.Bundle_Skill).SetActive(false);
        GetObject((int)Objects.Bundle_Soul).SetActive(false);
        GetObject((int)Objects.Bundle_SquareUpgrade).SetActive(false);
    }
    #endregion


    public enum Images
    {
        Image_SwipePlace
    }

    public enum Texts
    {
        Text_Gold
    }

    public enum Buttons
    {
        Button_BallUpgrade, Button_Shop, Button_Skill, Button_Soul, Button_SquareUpgrade
    }

    public enum Objects
    {
        Trans_BundleStartTweeing, Trans_BundleEndTweeing, Bundle_BallUpgrade, Bundle_Shop, Bundle_Skill, Bundle_Soul, Bundle_SquareUpgrade
    }
}

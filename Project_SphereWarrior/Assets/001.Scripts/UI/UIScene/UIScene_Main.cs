using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UIScene_Main : UIScene
{
    private Vector2 dragPointOne;
    private Sequence sequence;
    private RectTransform nowBundled;
    private int nowBundleIndex = -1;

    [SerializeField ]
    private float bundleSpeed;


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
        BindEvent(GetButton((int)Buttons.Button_BallDamageUpgrade).gameObject, Managers.Game.upgrade.BallDamageUpgrade);
        BindEvent(GetButton((int)Buttons.Button_BallSpeedUpgrade).gameObject, Managers.Game.upgrade.BallSpeedUpgrade);
        BindEvent(GetButton((int)Buttons.Button_BallCountUpgrade).gameObject, Managers.Game.upgrade.BallCountUpgrade);
        BindEvent(GetButton((int)Buttons.Button_BallCriticalUpgrade).gameObject, Managers.Game.upgrade.BallCriticalDamageUpgrade);
        BindEvent(GetButton((int)Buttons.Button_BallCriticalPercentageUpgrade).gameObject, Managers.Game.upgrade.BallCriticalPercentageUpgrade);

        CloseAllBundle();
        Managers.UI.SceneUI = this;
        RedrawUI();
        return true;
    }

    public override void RedrawUI()
    {
        GetText((int)Texts.Text_Gold).text = Managers.Game.player.inGameGoldString;
        GetText((int)Texts.Text_Cash).text = $"{Managers.Game.player.cash}";
        GetText((int)Texts.Text_Level).text = $"Level {Managers.Game.world.worldLevel}";

        //데미지
        GetText((int)Texts.Text_BallDamageLevel).text = $"Level {Managers.Game.player.ballDamageLevel}";
        GetText((int)Texts.Text_BallNowDamage).text = $"{Util.FloatToSymbolString(Managers.Game.player.currentBallDamage)}";
        GetText((int)Texts.Text_BallUpgradeDamage).text = $"{Util.FloatToSymbolString(Managers.Game.player.afterBallDamage)}";
        GetText((int)Texts.Text_BallDamageUpgradeCost).text = $"{Util.FloatToSymbolString(Managers.Game.upgrade.currentBallDamageUpgradeCost)}";

        //스피드
        GetText((int)Texts.Text_BallSpeedLevel).text = $"Level {Managers.Game.player.ballSpeedLevel}";
        GetText((int)Texts.Text_BallNowSpeed).text = $"{Util.FloatToSymbolString(Managers.Game.player.currentBallSpeed)}";
        GetText((int)Texts.Text_BallUpgradeSpeed).text = $"{Util.FloatToSymbolString(Managers.Game.player.afterBallSpeed)}";
        GetText((int)Texts.Text_BallSpeedUpgradeCost).text = $"{Util.FloatToSymbolString(Managers.Game.upgrade.currentBallSpeedUpgradeCost)}";

        //카운트
        GetText((int)Texts.Text_BallCountLevel).text = $"Level {Managers.Game.player.ballCountLevel}";
        GetText((int)Texts.Text_BallNowCount).text = $"{Util.FloatToSymbolString(Managers.Game.player.ballCount)}";
        GetText((int)Texts.Text_BallUpgradeCount).text = $"{Util.FloatToSymbolString(Managers.Game.player.ballCount + 1)}";
        GetText((int)Texts.Text_BallCountUpgradeCost).text = $"{Util.FloatToSymbolString(Managers.Game.upgrade.currentBallCountUpgradeCost)}";

        //크리티컬 데미지
        GetText((int)Texts.Text_BallCriticalLevel).text = $"Level {Managers.Game.player.ballCriticalDamageLevel}";
        GetText((int)Texts.Text_BallNowCritical).text = $"{Managers.Game.player.ballCriticalDamage}";
        GetText((int)Texts.Text_BallUpgradeCritical).text = $"{Managers.Game.player.ballCriticalDamage + 0.05f}";
        GetText((int)Texts.Text_BallCriticalUpgradeCost).text = $"{Util.FloatToSymbolString(Managers.Game.upgrade.currentBallCriticalDamgeUpgradeCost)}";

        //크리티컬 퍼센테이지
        GetText((int)Texts.Text_BallCriticalPercentageLevel).text = $"Level {Managers.Game.player.ballCriticalPercentageLevel}";
        GetText((int)Texts.Text_BallNowCriticalPercentage).text = $"{Managers.Game.player.ballCriticalPercentage}";
        GetText((int)Texts.Text_BallUpgradeCriticalPercentage).text = $"{Managers.Game.player.ballCriticalPercentage + 5f}";
        GetText((int)Texts.Text_BallCriticalPercentageUpgradeCost).text = $"{Util.FloatToSymbolString(Managers.Game.upgrade.currentBallCriticalPercentageUpgradeCost)}";
    }

    #region Swipe


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

        if (nowBundleIndex != (int)Objects.Bundle_BallUpgrade)
        {
            if(nowBundleIndex == -1)
                OpenBundle(bundle);

            else
                ChangeBundle(bundle);
            nowBundleIndex = (int)Objects.Bundle_BallUpgrade;
            return;
        } 

        else
        {
            CloseBundle(bundle);
            nowBundleIndex = -1;
        }
    }


    private void OnClick_Shop()
    {
        RectTransform bundle = GetObject((int)Objects.Bundle_Shop).GetComponent<RectTransform>();

        if (nowBundleIndex != (int)Objects.Bundle_Shop)
        {
            if (nowBundleIndex == -1)
                OpenBundle(bundle);

            else
                ChangeBundle(bundle);
            nowBundleIndex = (int)Objects.Bundle_Shop;
            return;
        }

        else
        {
            CloseBundle(bundle);
            nowBundleIndex = -1;
        }
    }

    private void OnClick_Skill()
    {
        RectTransform bundle = GetObject((int)Objects.Bundle_Skill).GetComponent<RectTransform>();

        if (nowBundleIndex != (int)Objects.Bundle_Skill)
        {
            if (nowBundleIndex == -1)
                OpenBundle(bundle);

            else
                ChangeBundle(bundle);
            nowBundleIndex = (int)Objects.Bundle_Skill;
            return;
        }

        else
        {
            CloseBundle(bundle);
            nowBundleIndex = -1;
        }
    }

    private void OnClick_Soul()
    {
        RectTransform bundle = GetObject((int)Objects.Bundle_Soul).GetComponent<RectTransform>();

        if (nowBundleIndex != (int)Objects.Bundle_Soul)
        {
            if (nowBundleIndex == -1)
                OpenBundle(bundle);

            else
                ChangeBundle(bundle);
            nowBundleIndex = (int)Objects.Bundle_Soul;
            return;
        }

        else
        {
            CloseBundle(bundle);
            nowBundleIndex = -1;
        }
    }

    private void OnClick_SquareUpgrade()
    {
        RectTransform bundle = GetObject((int)Objects.Bundle_SquareUpgrade).GetComponent<RectTransform>();

        if (nowBundleIndex != (int)Objects.Bundle_SquareUpgrade)
        {
            if (nowBundleIndex == -1)
                OpenBundle(bundle);

            else
                ChangeBundle(bundle);
            nowBundleIndex = (int)Objects.Bundle_SquareUpgrade;
            return;
        }

        else
        {
            CloseBundle(bundle);
            nowBundleIndex = -1;
        }
    }

    private void OpenBundle(RectTransform _bundle)
    {
        nowBundled = _bundle;
        _bundle.position = GetObject((int)Objects.Trans_BundleStartTweeing).transform.position;
        _bundle.gameObject.SetActive(true);
        DOTween.Kill($"{nameof(CloseBundle)}");
        sequence = DOTween.Sequence().SetId($"{nameof(OpenBundle)}");
        sequence.Join(_bundle.DOMove(GetObject((int)Objects.Trans_BundleEndTweeing).transform.position, bundleSpeed));
        sequence.AppendCallback(() => { sequence = null; });
        sequence.Play();
    }

    private void ChangeBundle(RectTransform _bundle)
    {
        if(nowBundled != null)
            nowBundled.gameObject.SetActive(false);
        nowBundled = _bundle;
        _bundle.transform.position = GetObject((int)Objects.Trans_BundleEndTweeing).transform.position;
        _bundle.gameObject.SetActive(true);
    }

    private void CloseBundle(RectTransform _bundle)
    {
        DOTween.Kill($"{nameof(OpenBundle)}");
        sequence = DOTween.Sequence().SetId($"{nameof(CloseBundle)}");
        sequence.Join(_bundle.DOMove(GetObject((int)Objects.Trans_BundleStartTweeing).transform.position, bundleSpeed));
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
        Text_Gold, Text_Cash, Text_Level,
        Text_BallNowDamage, Text_BallDamageLevel, Text_BallUpgradeDamage, Text_BallDamageUpgradeCost,
        Text_BallNowSpeed, Text_BallSpeedLevel, Text_BallUpgradeSpeed, Text_BallSpeedUpgradeCost,
        Text_BallCountLevel, Text_BallNowCount, Text_BallUpgradeCount, Text_BallCountUpgradeCost,
        Text_BallNowCritical, Text_BallCriticalLevel, Text_BallUpgradeCritical, Text_BallCriticalUpgradeCost,
        Text_BallNowCriticalPercentage, Text_BallCriticalPercentageLevel, Text_BallUpgradeCriticalPercentage, Text_BallCriticalPercentageUpgradeCost,

    }

    public enum Buttons
    {
        Button_BallUpgrade, Button_Shop, Button_Skill, Button_Soul, Button_SquareUpgrade, Button_Setting, Button_Mission,
        Button_BallDamageUpgrade , Button_BallSpeedUpgrade, Button_BallCountUpgrade, Button_BallCriticalUpgrade, Button_BallCriticalPercentageUpgrade
    }

    public enum Objects
    {
        Trans_BundleStartTweeing, Trans_BundleEndTweeing, Bundle_BallUpgrade, Bundle_Shop, Bundle_Skill, Bundle_Soul, Bundle_SquareUpgrade
    }
}

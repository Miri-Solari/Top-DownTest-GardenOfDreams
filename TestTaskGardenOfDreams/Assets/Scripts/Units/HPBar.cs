using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour //самый простой HPBar
{
    [SerializeField] BaseUnit unit;
    [SerializeField] Image HP;
    [SerializeField] private float _maxHP;


    private void Start()
    {
        HP.type = Image.Type.Filled;
        HP.fillMethod = Image.FillMethod.Horizontal;
        _maxHP = unit.HP;
    }


    private void Update()
    {
        HP.fillAmount = unit.HP / _maxHP;
    }

}
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private PlayerStats stats;
    [SerializeField] private PlayerController player;
    [SerializeField] private DialogueManager dislogue;
    [SerializeField] private CanvasScript canvas;
    [SerializeField] private SpellBook spell;
    [SerializeField] private Interactive intercative;
    [SerializeField] private GameObject[] Panels = new GameObject[3];
    [SerializeField] private Text[] moneyText = new Text[3];

    private void Awake()
    {
        Debug.Log(spell);
    }
    public override void InstallBindings()
    {
        Container.Bind<Inventory>().FromInstance(inventory).AsSingle();
        Container.Bind<PlayerStats>().FromInstance(stats).AsSingle();
        Container.Bind<PlayerController>().FromInstance(player).AsSingle();
        Container.Bind<DialogueManager>().FromInstance(dislogue).AsSingle();
        Container.Bind<CanvasScript>().FromInstance(canvas).AsSingle();
        Container.Bind<SpellBook>().FromInstance(spell).AsSingle();
        Container.Bind<Interactive>().FromInstance(intercative).AsSingle();


        Container.Bind<GameObject[]>().FromInstance(Panels).AsSingle();
        Container.Bind<Text[]>().FromInstance(moneyText).AsSingle();
    }
}
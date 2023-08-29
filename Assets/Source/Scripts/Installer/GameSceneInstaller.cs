using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private PlayerStats stats;
    [SerializeField] private PlayerController player;
    [SerializeField] private DialogueManager dislogue;
    [SerializeField] private CanvasScript canvas;
    [SerializeField] private SpellBook spell;
    public override void InstallBindings()
    {
        Container.Bind<Inventory>().FromInstance(inventory).AsSingle();
        Debug.Log(inventory + "are insalled");
        Container.Bind<PlayerStats>().FromInstance(stats).AsSingle();
        Debug.Log(stats + "are insalled");
        Container.Bind<PlayerController>().FromInstance(player).AsSingle();
        Debug.Log(player + "are insalled");
        Container.Bind<DialogueManager>().FromInstance(dislogue).AsSingle();
        Debug.Log(dislogue + "are insalled");
        Container.Bind<CanvasScript>().FromInstance(canvas).AsSingle();
        Debug.Log(canvas + "are insalled");
        Container.Bind<SpellBook>().FromInstance(spell).AsSingle();
    }
}
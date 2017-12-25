using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGamePanelHandler : MonoBehaviour {

    [SerializeField] float timeBetweenText;
    [SerializeField] GameObject readyText;
    [SerializeField] GameObject setText;
    [SerializeField] GameObject goText;

    private void OnEnable ()
    {
        StartRoutine();
	}
	
	public void StartRoutine()
    {
        readyText.SetActive(true);
        setText.SetActive(false);
        goText.SetActive(false);

        GameMaster.Instance.Pause(true);

        StartCoroutine(Ninja.Util.Func.WaitAndRunActionInRealTime(
            timeBetweenText,
            () => {
                readyText.SetActive(false);
                setText.SetActive(true);

                StartCoroutine(Ninja.Util.Func.WaitAndRunActionInRealTime(
                    timeBetweenText,
                    () => {
                        setText.SetActive(false);
                        goText.SetActive(true);

                        StartCoroutine(Ninja.Util.Func.WaitAndRunActionInRealTime(
                            timeBetweenText,
                            () => {
                                goText.SetActive(false);

                                GUIManager.Instance.EndPreGame();
                            }));
                    }));
            }));
    }
}

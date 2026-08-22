using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HackingManager : MonoBehaviour
{
    private const float m_hackingTimerDuration = 2.0f;

    private Enemy m_currentHackableEnemy = null;
    private bool m_isHacking = false;
    private float m_hackingTimer = 0.0f;
    private int m_currentBindingIndex = 0;
    private QuickTimeBinding m_requiredBinding = null;
    private List<QuickTimeBinding> m_generatedHackingSequence = new List<QuickTimeBinding>();

    [SerializeField] private List<QuickTimeSet> m_quickTimeSets = new List<QuickTimeSet>();

    public static HackingManager Instance
    {
        get; private set;
    }

    public bool IsHacking
    {
        get { return m_isHacking; }
    }

    public QuickTimeBinding RequiredBinding
    {
        get { return m_requiredBinding; }
    }

    public float HackingTimer
    {
        get { return m_hackingTimer; }
    }

    public void SetCurrentHackableEnemy(Enemy _currentEnemy)
    {
        m_currentHackableEnemy = _currentEnemy;
        Debug.Log("Enemy set");
    }

    public void TriggerHacking()
    {
        if(m_currentHackableEnemy != null && GameManager.Instance.CurrentGameState != GameStates.HACKING)
        {
            if(m_currentHackableEnemy.IsLidOpenable == true)
            {
                GameManager.Instance.SetGameState(GameStates.HACKING);
                UIManager.Instance.SetUIState(UIStates.HACKING);
                Debug.Log("Hacking started");
                StartHackingSequence();
            }
        }
    }

    private void GenerateHackingSequence(int _setAmount)
    {
        m_generatedHackingSequence.Clear();

        if(m_quickTimeSets.Count <= 0 || _setAmount <= 0) {
            return;
        } 

        for(int i = 0; i < _setAmount; i++)
        {
            // choose random set
            QuickTimeSet chosenSet = m_quickTimeSets[Random.Range(0, m_quickTimeSets.Count)];

            // define amount of bindings to choose
            int amountChosenBindings = Random.Range(chosenSet.MinAmountChosenBindings, chosenSet.MaxAmountChosenBindings + 1);

            // choose random bindings from set
            for(int j = 0; j < amountChosenBindings; j++)
            {
                if(chosenSet.Bindings.Count == 0)
                {
                    continue;
                }

                QuickTimeBinding chosenBinding = chosenSet.Bindings[Random.Range(0, chosenSet.Bindings.Count)];

                m_generatedHackingSequence.Add(chosenBinding);
            }
        }
    }

    private void StartHackingSequence()
    {
        int randomAmount = Random.Range(3, 6);

        GenerateHackingSequence(randomAmount);

        m_currentBindingIndex = 0;

        m_hackingTimer = m_hackingTimerDuration;
        m_isHacking = true;
    }

    private void Hacking()
    {
        if(m_generatedHackingSequence == null || m_currentBindingIndex >= m_generatedHackingSequence.Count)
        {
            return;
        }

        m_hackingTimer -= Time.deltaTime;
        Debug.Log("Timer " + m_hackingTimer);

        m_requiredBinding = m_generatedHackingSequence[m_currentBindingIndex];

        Debug.Log("Press " + m_requiredBinding.BindingInputActionReference.name);

        // check every possible binding and fail if wrong binding was pressed
        foreach(QuickTimeSet quickTimeSet in m_quickTimeSets)
        {
            foreach(QuickTimeBinding quickTimeBinding in quickTimeSet.Bindings)
            {
                if(quickTimeBinding.BindingInputActionReference.action.WasPressedThisFrame() == true)
                {
                    if(quickTimeBinding == m_requiredBinding)
                    {
                        OnRequiredBindingPressed();
                        return;
                    }
                    else
                    {
                        Debug.Log("Game Over");
                        GameManager.Instance.SetGameState(GameStates.GAMEOVER);
                        break;
                    }
                }
            }
        }

        if(m_hackingTimer <= 0.0f)
        {
            Debug.Log("Game Over");
            GameManager.Instance.SetGameState(GameStates.GAMEOVER);
            return;
        }
    }

    private void OnRequiredBindingPressed()
    {
        m_currentBindingIndex++;

        if(m_currentBindingIndex >= m_generatedHackingSequence.Count)
        {
            // hacking done
            CharController.Instance.SetCharState(m_currentHackableEnemy.CorrespondingCharState);
            GameManager.Instance.SetGameState(GameStates.RUNNING);
            m_isHacking = false;
            UIManager.Instance.ChooseUIStateBasedOnCharState();
            return;
        }

        m_hackingTimer = m_hackingTimerDuration;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if(GameManager.Instance.CurrentGameState == GameStates.HACKING)
        {
            Hacking();
        }
    }
}

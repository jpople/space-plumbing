using TMPro;
using UnityEngine;

public class ManualController : MonoBehaviour {
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] TextMeshProUGUI pageNumberDisplay;
    AudioSource source;

    int currentPage = 1;

    private void Start() {
        source = GetComponent<AudioSource>();
        mainText.text = ManualMDParser.ParseFile();
        pageNumberDisplay.text = $"{mainText.pageToDisplay}";
        RefreshPageNumber();
    }

    public void ChangePage(int change) {
        int newPage = currentPage + change;
        GoToPage(newPage);
        source.Play();
    }

    public void GoToFirst() {
        GoToPage(1);
    }

    public void GoToLast() {
        GoToPage(mainText.textInfo.pageCount);
    }

    public void GoToPage(int pageNumber) {
        currentPage = Mathf.Clamp(pageNumber, 1, mainText.textInfo.pageCount);
        mainText.pageToDisplay = currentPage;
        RefreshPageNumber();
    }

    void RefreshPageNumber() {
        pageNumberDisplay.text = $"{currentPage}";
    }
}

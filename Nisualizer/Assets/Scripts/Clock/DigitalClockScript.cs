using System;
using TMPro;
using UnityEngine;

namespace Clock
{
    public class DigitalClockScript : MonoBehaviour
    {
        [SerializeField] private TMP_Text _time;
        [SerializeField] private TMP_Text _date;
        [SerializeField] private TMP_Text _day;
        [SerializeField] private string _timeFormat = "HH:mm:ss";
        [SerializeField] private string _dateFormat = "dd/MM/yyyy";

        private void Reset()
        {
            _time = transform.Find("Time").GetComponent<TMP_Text>();
            _date = transform.Find("Date").GetComponent<TMP_Text>();
            _day = transform.Find("Day").GetComponent<TMP_Text>();
        }

        private void Update()
        {
            _time.text = DateTime.Now.ToString(_timeFormat);
            _date.text = DateTime.Now.ToString(_dateFormat);
            _day.text = DateTime.Now.DayOfWeek.ToString();
        }
    }
}
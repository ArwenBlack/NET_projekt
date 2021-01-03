
import neurokit2 as nk
import pandas as pd


def generate(time: int, rate: int):
    ecg1 = nk.ecg_simulate(duration=time, sampling_rate= rate, method = "ecgsyn") # duration - ile sekund  #sampling rate - ile punktów na sekunde
    ecg2 = nk.ecg_simulate(duration=time, sampling_rate= rate, method = "ecgsyn")
    ecg3 = nk.ecg_simulate(duration=time, sampling_rate= rate, method="ecgsyn")
    ecg4 = nk.ecg_simulate(duration=time, sampling_rate= rate, method="daubechies")
    emg1 = nk.emg_simulate(duration=time, sampling_rate= rate, random_state=1, burst_number=3)
    emg2 = nk.emg_simulate(duration=time, sampling_rate= rate, random_state=2, burst_number=3)
    emg3 = nk.emg_simulate(duration=time, sampling_rate= rate, random_state=3, burst_number=3)
    emg4 = nk.emg_simulate(duration=time, sampling_rate= rate, random_state=4, burst_number=3)
    # sa jeszcze analogicznie fajne wykresy dla ppg, rsp i eda
    nk.signal_plot(ecg1, sampling_rate=rate)
    nk.signal_plot(ecg2, sampling_rate=rate)

    data = pd.DataFrame({'ecg1': ecg1,
                         'ecg2': ecg2,
                         'ecg3': ecg3,
                         'ecg4': ecg4,
                         'emg1': emg1,
                         'emg2': emg2,
                         'emg3': emg3,
                         'emg4': emg4,
                         })
    return data


data = generate(1800, 10)
data.to_csv(r'ecg_emg_10Hz_30min.csv')
data = generate(1800, 100)
data.to_csv(r'ecg_emg_100Hz_30min.csv')
data = generate(1800, 250)
data.to_csv(r'ecg_emg_250Hz_30min.csv')
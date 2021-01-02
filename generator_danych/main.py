
import neurokit2 as nk
import pandas as pd


def generate(time: int, rate: int):
    ecg1 = nk.ecg_simulate(duration=time, sampling_rate= rate, method = "daubechies") # duration - ile sekund  #sampling rate - ile punktów na sekunde
    ecg2 = nk.ecg_simulate(duration=time, sampling_rate= rate, method = "daubechies")
    ecg3 = nk.ecg_simulate(duration=time, sampling_rate= rate, method="daubechies")
    ecg4 = nk.ecg_simulate(duration=time, sampling_rate= rate, method="daubechies")
    emg1 = nk.emg_simulate(duration=time, sampling_rate= rate, burst_number=3)
    emg2 = nk.emg_simulate(duration=time, sampling_rate= rate, burst_number=3)
    emg3 = nk.emg_simulate(duration=time, sampling_rate= rate,  burst_number=3)
    emg4 = nk.emg_simulate(duration=time, sampling_rate= rate, burst_number=3)
    # sa jeszcze analogicznie fajne wykresy dla ppg, rsp i eda
    #signal, info = nk.emg_process(emg4, sampling_rate=250)  #możan sobie wyświetlić ładnie wykresy
    #nk.emg_plot(signal, sampling_rate=250)

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

data = generate(7200, 10)
data.to_csv(r'ecg_emg_10Hz_2h.csv')
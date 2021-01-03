
from neurokit2 import ecg_simulate
from neurokit2 import emg_simulate

from pandas import DataFrame


def generate_ecg(time, rate):
    ecg = ecg_simulate(duration=time, sampling_rate= rate, method = "ecgsyn")

    return ecg

def generate_emg(time, rate):
    emg = emg_simulate(duration=time, sampling_rate= rate, random_state=1, burst_number=3)

    return emg

ecg = generate_ecg(10, 10)
emg = generate_emg(10, 10)

data = DataFrame({"ecg": ecg, "emg": emg})
data.to_csv(r"data.csv")


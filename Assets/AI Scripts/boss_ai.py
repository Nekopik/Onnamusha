import tensorflow as tf
import numpy as np
import pandas as pd
import os

DATA_FOLDER = "data"

LABEL_VALUES = {
    "excellent": 1.9,
    "good": 1.6,
    "decent": 1.2,
    "bad": 0.9
}

EPOCHS = 500
LEARNING_RATE = 0.01

def load_all_data(folder_path):
    all_X = []
    all_y = []

    for file in os.listdir(folder_path):
        file_lower = file.lower()

        for label_name, skill_value in LABEL_VALUES.items():
            if label_name in file_lower:

                path = os.path.join(folder_path, file)

                df = pd.read_csv(path, header=None)

                if df.shape[1] != 5:
                    raise ValueError(f"{file} does not have 5 columns.")

                X = df.values  # melee, range, duration, hp_loss, arrived

                y = np.full((len(X), 1), skill_value)

                all_X.append(X)
                all_y.append(y)

    if len(all_X) == 0:
        raise ValueError("No matching CSV files found.")

    X_final = np.vstack(all_X)
    y_final = np.vstack(all_y)

    return X_final.astype(np.float32), y_final.astype(np.float32)

X, y = load_all_data(DATA_FOLDER)

print("Loaded samples:", X.shape[0])
print("Input shape:", X.shape)
print("Label shape:", y.shape)

inputs = tf.keras.Input(shape=(5,))

x = tf.keras.layers.Dense(32, activation="relu")(inputs)
x = tf.keras.layers.Dense(16, activation="relu")(x)

skill_raw = tf.keras.layers.Dense(1, activation="sigmoid")(x)

skill_output = tf.keras.layers.Rescaling(
    scale=1.2,
    offset=0.8,
    name="skill_modifier"
)(skill_raw)

model = tf.keras.Model(inputs=inputs, outputs=skill_output)

model.compile(
    optimizer=tf.keras.optimizers.Adam(learning_rate=LEARNING_RATE),
    loss="mse"
)

model.summary()

model.fit(
    X,
    y,
    epochs=EPOCHS,
    batch_size=16,
    validation_split=0.2,
    verbose=1
)

model.save("boss_ai_brain")

print("Model saved successfully.")
import tensorflow as tf
import numpy as np

np.random.seed()

def generate_passive(n):
    data = []
    for _ in range(n):
        data.append([
            np.random.uniform(6.5, 9.5),      # durationOfMobFight
            np.random.randint(0, 2),          # mobMeleeAttacks
            np.random.randint(4, 8),          # mobRangeAttacks
            np.random.uniform(0.4, 0.7),      # playerLossHP
            np.random.randint(5, 20),         # allMobMeleeAttacks
            np.random.randint(30, 70),        # allMobRangeAttacks
            np.random.uniform(7.0, 9.0),      # averageFightDuration
            np.random.uniform(0.4, 0.7)       # averagePlayerLossHPPerFight
        ])
    return data


def generate_aggressive(n):
    data = []
    for _ in range(n):
        data.append([
            np.random.uniform(1.5, 4.5),      # durationOfMobFight (<5)
            np.random.randint(5, 10),         # high melee
            np.random.randint(0, 2),          # low ranged
            np.random.uniform(0.6, 0.9),      # high HP loss
            np.random.randint(50, 100),       # high total melee
            np.random.randint(0, 15),         # low total ranged
            np.random.uniform(2.0, 4.0),      # short avg fight
            np.random.uniform(0.6, 0.9)       # high avg HP loss
        ])
    return data


def generate_mixed(n):
    data = []
    for _ in range(n):
        data.append([
            np.random.uniform(4.5, 6.5),
            np.random.randint(2, 5),
            np.random.randint(2, 5),
            np.random.uniform(0.45, 0.65),
            np.random.randint(20, 50),
            np.random.randint(15, 40),
            np.random.uniform(5.0, 6.5),
            np.random.uniform(0.45, 0.65)
        ])
    return data


passive_samples = generate_passive(20)
aggressive_samples = generate_aggressive(20)
mixed_samples = generate_mixed(20)

X = np.array(
    passive_samples + aggressive_samples + mixed_samples,
    dtype=np.float32
)

y = np.array(
    [0]*20 + [1]*20 + [0]*20,
    dtype=np.float32
)

indices = np.arange(len(X))
np.random.shuffle(indices)
X = X[indices]
y = y[indices]


X[:,0] /= 10.0   # durationOfMobFight
X[:,1] /= 10.0   # mobMeleeAttacks
X[:,2] /= 10.0   # mobRangeAttacks
X[:,4] /= 100.0  # allMobMeleeAttacks
X[:,5] /= 100.0  # allMobRangeAttacks
X[:,6] /= 10.0   # averageFightDuration

model = tf.keras.Sequential([
    tf.keras.layers.Input(shape=(8,)),
    tf.keras.layers.Dense(32, activation='relu'),
    tf.keras.layers.Dense(16, activation='relu'),
    tf.keras.layers.Dense(8, activation='relu'),
    tf.keras.layers.Dense(1, activation='sigmoid')
])

model.compile(
    optimizer='adam',
    loss='binary_crossentropy',
    metrics=['accuracy']
)

model.fit(
    X,
    y,
    epochs=300,
    verbose=1
)

model.save("mob_ai")

print("Model saved successfully.")
import os

def say_hello():
    return "python method call successful, hello"

def test(message):
    directory = os.getcwd()
    return message +": " +directory
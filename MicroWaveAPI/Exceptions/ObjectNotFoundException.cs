namespace MicroWaveAPI.Exceptions;

using System;
public class ObjectNotFoundException(string message) : Exception(message);
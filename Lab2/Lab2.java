package com.company;

import java.util.HashMap;
import java.util.*;

public class Main
{
    // Variant 8
    // Q = {q0, q1, q2, q3, q4}
    // E = {a, b}, F = {q3}
    // (q0, a) = q1
    // (q1, b) = q2
    // (q2, b) = q0
    // (q3, a) = q4
    // (q4, a) = q0
    // (q2, a) = q3
    // (q1, b) = q1

    public static List<String> alpha = List.of("a", "b");
    public static Map<String, Map<String, String>> nfaRules = new HashMap<>();

    public static void main(String[] args)
    {
        nfaRules.put("q0", Map.of("a", "q1"));
        nfaRules.put("q1", Map.of("b", "q1 q2"));
        nfaRules.put("q2", Map.of("b", "q0", "a", "q3"));
        nfaRules.put("q3", Map.of("a", "q4"));
        nfaRules.put("q4", Map.of("a", "q0"));

        var convertedDfa = ConvertDFA();
        PrintDFA(convertedDfa);
    }

    public static Map<String, Map<String, String>> ConvertDFA()
    {
        var dfaRules = new HashMap<>(nfaRules);

        for (var sRules : nfaRules.entrySet())
        {
            for (var rule : sRules.getValue().entrySet())
            {
                if (!dfaRules.containsKey(rule.getValue()))
                {
                    String newState = rule.getValue();
                    Map<String, String> newRules = new HashMap<>();
                    for (String word : alpha)
                    {
                        String endString = "";
                        for (String state : newState.split(" "))
                        {
                            var s = nfaRules.get(state).get(word);
                            if (s != null)
                            {
                                endString += " ";
                                endString += s;
                            }
                        }
                        var strState = endString.trim();
                        if (!strState.isEmpty() && !strState.isBlank())
                            newRules.put(word, strState);
                    }
                    dfaRules.put(newState, newRules);
                }
            }
        }
        return dfaRules;
    }

    public static void PrintDFA(Map<String, Map<String, String>> rules)
    {
        for (var sRules : rules.entrySet())
        {
            for (var rule : sRules.getValue().entrySet())
            {
                System.out.printf("([%s], %s) => %s\n", sRules.getKey(), rule.getKey(), rule.getValue());
            }
        }
    }
}
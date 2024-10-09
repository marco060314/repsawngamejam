using Godot;
using System;

public partial class Counter : RichTextLabel
{
    public String prefix;
    public int counter;
    public Counter(String prefix){
        this.prefix=prefix;
        Text=prefix;
    }
    public void add(int dx){
        counter+=dx;
        Text=prefix+" "+counter;
    }
    public void set(int counter){
        this.counter=counter;
        Text=prefix+" "+counter;
    }
}

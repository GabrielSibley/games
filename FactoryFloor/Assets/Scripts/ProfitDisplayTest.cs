using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProfitDisplayTest : MonoBehaviour {

    public float xScale = 0.1f;
    public float eventSize = 1;
    public float eventWidth = 1;

    private class MoneyEvent
    {
        public int tick;
        public int amount;

        public int GetDuration(float size, float whRatio)
        {
            return Mathf.CeilToInt(Mathf.Sqrt(2 * Mathf.Abs(amount) * size * whRatio));            
        }

        public MoneyEvent(int tick, int amount)
        {
            this.tick = tick;
            this.amount = amount;
        }
    }

    private class ContractSpec
    {
        public int start;
        public int jobLength;
        public int quantity;
        public int[] machineCosts;
        public int supplyCost;
        public int payment;

        public ContractSpec(int start, int jobLength, int quantity, int[] machineCosts, int supplyCost, int payment)
        {
            this.start = start;
            this.jobLength = jobLength;
            this.quantity = quantity;
            this.machineCosts = machineCosts;
            this.supplyCost = supplyCost;
            this.payment = payment;
        }
    }

    private List<MoneyEvent> events = new List<MoneyEvent>();

    private ContractSpec[] contracts = new ContractSpec[]
    {
        new ContractSpec(5, 90, 40, new int[] {20, 5, 5}, 3, 5),
        new ContractSpec(600, 50, 25, new int[] {10, 10}, 2, 6),
        new ContractSpec(1200, 180, 18, new int[] {10, 20, 10, 15}, 6, 18),
        new ContractSpec(1800, 120, 60, new int[] { }, 2, 6)
    };

    void InitData()
    {
        foreach(ContractSpec con in contracts)
        {
            //Buy machinery every 45 ticks until all machinery is had
            for(int i = 0; i < con.machineCosts.Length; i++)
            {
                events.Add(new MoneyEvent(con.start + i * 45, -con.machineCosts[i]));
            }
            int machineryCompleteTime = con.start + con.machineCosts.Length * 45 + 30;
            //Make stuff and sell it
            for(int i = 0; i < con.quantity; i++)
            {
                //Buy supplies
                events.Add(new MoneyEvent(machineryCompleteTime + con.jobLength * i, -con.supplyCost));
                //Sell products
                events.Add(new MoneyEvent(machineryCompleteTime + con.jobLength * (i + 1), con.payment));
            }
        }
        //Sort events by time
        events.Sort((x, y) => x.tick.CompareTo(y.tick));
    }

	void OnDrawGizmos()
    {
        if(events.Count == 0)
        {
            InitData();
        }
        int tick = 0;
        int eventIndex = 0;
        int totalThisTick = 0;
        float totalActive = 0;
        int finalTick = events.Count > 0 ? events[events.Count - 1].tick + 120 : 120;

        float smoothedSaw = 0;

        List<MoneyEvent> trackedEvents = new List<MoneyEvent>();

        while(tick <= finalTick)
        {
            //Track events on this tick
            totalThisTick = 0;
            while (eventIndex < events.Count && events[eventIndex].tick == tick)
            {
                trackedEvents.Add(events[eventIndex]);
                totalThisTick += events[eventIndex].amount;
                eventIndex++;                
            }

            //proc active events
            float totalActiveLastTick = totalActive;
            totalActive = 0;            
            for(int i = 0; i < trackedEvents.Count; i++)
            {
                int dur = trackedEvents[i].GetDuration(eventSize, eventWidth);
                if(trackedEvents[i].tick + dur < tick)
                {
                    //event expired
                    trackedEvents.RemoveAt(i);
                    i--;
                }
                else
                {
                    float t = (tick - trackedEvents[i].tick) / (float)dur;
                    float h = trackedEvents[i].amount * eventSize / dur;
                    totalActive += h * (1 - t);
                }
            }

            //draw
            //Gizmos.color = Color.white;
            //Gizmos.DrawRay(new Vector2(tick * xScale, 0), new Vector2(0, totalThisTick));
            Gizmos.color = Color.green;
            float lastSmoothedSaw = smoothedSaw;
            smoothedSaw = smoothedSaw * 0.95f + totalActive * 0.05f;

            float lastY = lastSmoothedSaw;
            float curY = smoothedSaw;

            Gizmos.DrawLine(new Vector2((tick - 1) * xScale, lastY), new Vector2(tick * xScale, curY));
                
            tick++;        
        }
    }
}

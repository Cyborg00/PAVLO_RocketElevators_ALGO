
                /*--------------WELCOME-TO-ROCKET-ELEVATORS-RESIDENTIAL-CONTROLLER-IN-C#------------------*/
                                        //SCROLL DOWN TO SEE THE SCENARION

using System;
using System.Collections.Generic;
using System.Threading;

namespace CommercialController
{
    /// <summary>
    /// Class that takes in the user request and process it.
    /// </summary>
    public class ElevatorController
    {
        public int nbrFloors;
        public int nbrElevators;
        public int nbrColumns;
        public string userDirection;
        public Battery battery;
        public List<int> shortList;

        public ElevatorController(int nbrFloors, int nbrColumns, int nbrElevators, string userDirection)
        {
            this.nbrFloors = nbrFloors;
            this.nbrColumns = nbrColumns;
            this.nbrElevators = nbrElevators;
            this.userDirection = userDirection;
            this.battery = new Battery(this.nbrColumns);
        }


        /// <summary>
        /// This section is dedicated to the people desiering to go down.
        /// </summary>
        public Elevator requestElevator(int floorNbr, int requestedFloor)
        {
            Thread.Sleep(500);
            Console.WriteLine("REQUEST AN ELEVATOR TO FLOOR : " + floorNbr);
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>");
            Thread.Sleep(500);
            Console.WriteLine("CALL BUTTON LIGHT ON");
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>");
            var column = battery.Find_best_column(floorNbr);
            userDirection = "DOWN";
            var elevator = column.Find_Requested_elevator(floorNbr, userDirection);
            if (elevator.elevatorFloor > floorNbr)
            {
                elevator.sendRequest(floorNbr, column.colNbr);
                elevator.sendRequest(requestedFloor, column.colNbr);
            }

            else if (elevator.elevatorFloor < floorNbr)
            {
                elevator.moveDown(requestedFloor, column.colNbr);
                elevator.sendRequest(floorNbr, column.colNbr);
                elevator.sendRequest(requestedFloor, column.colNbr);
            }
            Console.WriteLine("CALL BUTTON LIGHT OFF");

            return elevator;
        }

        /// <summary>
        /// This section is dedicated to the people deserving to go up.
        /// </summary>
        public Elevator AssignElevator(int requestedFloor)
        {
            Thread.Sleep(500);
            Console.WriteLine("THE REQUESTED FLOOR IS: " + requestedFloor);
            Thread.Sleep(500);
            Console.WriteLine("CALL BUTTON LIGHT ON ");


            Column column = battery.Find_best_column(requestedFloor);
            userDirection = "UP";
            var floorNbr = 1;
            Elevator elevator = column.Find_Assign_elevator(requestedFloor, floorNbr, userDirection);

            elevator.sendRequest(floorNbr, column.colNbr);
            elevator.sendRequest(requestedFloor, column.colNbr);

            return elevator;
        }
    }

    /// <summary>
    /// In this section we can see all the operations that take place fot the door and movement.
    /// </summary>
    public class Elevator
    {
        public int elevatorNb;
        public string status;
        public int elevatorFloor;
        public string elevator_direction;
        public bool Sensor;
        public int FloorDisplay;
        public List<int> floor_list;

        public Elevator(int elevatorNb, string status, int elevatorFloor, string elevator_direction)
        {
            this.elevatorNb = elevatorNb;
            this.status = status;
            this.elevatorFloor = elevatorFloor;
            this.elevator_direction = elevator_direction;
            this.FloorDisplay = elevatorFloor;
            this.Sensor = true;
            this.floor_list = new List<int>();
        }

        /// <summary>
        /// In this section we assign an elevator based on the sendRequest received from the users.
        /// </summary>
        public void sendRequest(int requestedFloor, char colNbr)
        {
            floor_list.Add(requestedFloor);
            if (requestedFloor > elevatorFloor)
            {
                floor_list.Sort((a, b) => a.CompareTo(b));
            }
            else if (requestedFloor < elevatorFloor)
            {
                floor_list.Sort((a, b) => -1 * a.CompareTo(b));

            }

            operElevator(requestedFloor, colNbr);
        }


        /// <summary>
        /// In this section we divide the tasks depending on the directions requested.
        /// </summary>
        public void operElevator(int requestedFloor, char colNbr)
        {
            if (requestedFloor == elevatorFloor)
            {
                openDoors();
                this.status = "MOVING";

                this.floor_list.Remove(0);
            }
            else if (requestedFloor < this.elevatorFloor)
            {
                status = "MOVING";
                Console.WriteLine("CALL BUTTON LIGHT OFF");
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>");
                Console.WriteLine("Best Column : " + colNbr + " Best Elevator : " + this.elevatorNb + " " + status);
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>");
                this.elevator_direction = "DOWN";
                moveDown(requestedFloor, colNbr);
                this.status = "STOP";
                Console.WriteLine("Best Column : " + colNbr + " Best Elevator : " + this.elevatorNb + " " + status);

                this.openDoors();
                this.floor_list.Remove(0);
            }
            else if (requestedFloor > this.elevatorFloor)
            {
                Thread.Sleep(500);
                this.status = "MOVING";
                Console.WriteLine("CALL BUTTON LIGHT OFF");
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>");
                Console.WriteLine("Best Column : " + colNbr + " Best Elevator : " + this.elevatorNb + " " + status);
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>");
                this.elevator_direction = "UP";
                this.Move_up(requestedFloor, colNbr);
                this.status = "STOP";
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>");
                Console.WriteLine("Best Column :  " + colNbr + " Best Elevator : " + this.elevatorNb + " " + status);


                this.openDoors();

                this.floor_list.Remove(0);
            }

        }
        /// <summary>
        /// In this section we have the open and close the doors.
        /// </summary>
        public void openDoors()
        {
            Thread.Sleep(500);

            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>");

            Console.WriteLine("DOOR OPENED");
            Thread.Sleep(500);

            this.closeDoor();
        }
        public void closeDoor()
        {
            if (Sensor == true)
            {
                Console.WriteLine("DOOR CLOSED");
                Thread.Sleep(500);


                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>");
            }
            else if (Sensor == false)
            {
                openDoors();
            }
        }

        /// <summary>
        /// Move Up function.
        /// </summary>
        public void Move_up(int requestedFloor, char colNbr)
        {
            Console.WriteLine("The Column chosen is : " + colNbr + " The Best Elevator for you is : #" + elevatorNb + "  Current Floor : " + this.elevatorFloor);
            Thread.Sleep(500);
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>");
            while (this.elevatorFloor != requestedFloor)
            {
                this.elevatorFloor += 1;
                Console.WriteLine("The Column chosen is : " + colNbr + " The Best Elevator for you is : #" + elevatorNb + "  Floor : " + this.elevatorFloor);

                Thread.Sleep(500);
            }
        }


        /// <summary>
        /// Move Down function.
        /// </summary>
        public void moveDown(int requestedFloor, char colNbr)
        {
            Console.WriteLine("The Column chosen is : " + colNbr + " The Best Elevator for you is : #" + elevatorNb + "  Current Floor : " + this.elevatorFloor);
            Thread.Sleep(500);
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>");

            while (this.elevatorFloor != requestedFloor)
            {
                this.elevatorFloor -= 1;
                Console.WriteLine("The Column chosen is : " + colNbr + " The Best Elevator for you is : #" + elevatorNb + "  Floor : " + this.elevatorFloor);

                Thread.Sleep(500);
            }
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>");

        }

    }


    /// <summary>
    /// In this section we focus on finding the best column and we create a new elevator.
    /// </summary>
    public class Column
    {
        public char colNbr;
        public int nbrFloors;
        public int nbrElevators;
        public List<Elevator> elevator_list;
        public List<int> call_button_list;


        public Column(char colNbr, int nbrFloors, int nbrElevators)
        {
            this.colNbr = colNbr;
            this.nbrFloors = nbrFloors;
            this.nbrElevators = nbrElevators;
            elevator_list = new List<Elevator>();
            call_button_list = new List<int>();
            for (int i = 0; i < this.nbrElevators; i++)
            {
                Elevator elevator = new Elevator(i, "IDLE", 1, "UP");
                elevator_list.Add(elevator);
            }
        }


        /// <summary>
        /// In this section we find the best elevator for the users requesting to go up.
        /// </summary>
        public Elevator Find_Assign_elevator(int requestedFloor, int floorNbr, string userDirection)
        {

            foreach (var elevator in elevator_list)
                if (elevator.status == "IDLE")
                {
                    return elevator;
                }

            var bestElevator = 0;
            var shortest_distance = 1000;
            for (var i = 0; i < this.elevator_list.Count; i++)
            {
                var ref_distance = Math.Abs(elevator_list[i].elevatorFloor - elevator_list[i].floor_list[0]) + Math.Abs(elevator_list[i].floor_list[0] - 1);
                if (shortest_distance >= ref_distance)
                {
                    shortest_distance = ref_distance;
                    bestElevator = i;
                }
            }
            return elevator_list[bestElevator];
        }

        /// <summary>
        /// In this section we find the best elevator for the users requesting to go down.
        /// </summary>
        public Elevator Find_Requested_elevator(int requestedFloor, string userDirection)
        {
            var shortest_distance = 999;
            var bestElevator = 0;

            for (var i = 0; i < this.elevator_list.Count; i++)
            {
                var ref_distance = elevator_list[i].elevatorFloor - requestedFloor;

                if (ref_distance > 0 && ref_distance < shortest_distance)
                {
                    shortest_distance = ref_distance;
                    bestElevator = i;
                }
            }
            return elevator_list[bestElevator];
        }

    }


    /// <summary>
    /// This is the battery section to make sure that my column is created and that my elevator is going to work.
    /// </summary>
    public class Battery
    {
        public string battery_status;
        public int nbrColumns;
        public List<Column> column_list;
       

        public Battery(int nbrColumns)
        {
            this.nbrColumns = nbrColumns;
            this.battery_status = "ON";
            column_list = new List<Column>();



            char cols = 'A';
            for (int i = 0; i < this.nbrColumns; i++, cols++)
            {
                Column column = new Column(cols, 60, 5);
                column.colNbr = cols;
                column_list.Add(column);
            }
        }

        /// <summary>
        /// In this section we are looking for the best column depending on the request.
        /// </summary>
        public Column Find_best_column(int requestedFloor)
        {
            Column best_column = null;
            foreach (Column column in column_list)
            {
                if (requestedFloor > -5 && requestedFloor <= 0 || requestedFloor == 1)
                {
                    best_column = column_list[0];
                }
                else if (requestedFloor > 2 && requestedFloor <= 20 || requestedFloor == 1)
                {
                    best_column = column_list[1];
                }
                else if (requestedFloor > 21 && requestedFloor <= 40 || requestedFloor == 1)
                {
                    best_column = column_list[2];
                }
                else if (requestedFloor > 41 && requestedFloor <= 60 || requestedFloor == 1)
                {
                    best_column = column_list[3];
                }

            }
            return best_column;
        }
    }


    public class CommercialCS
    {
        public static void Main(string[] args)
        {
            ElevatorController controller = new ElevatorController(60, 4, 5, "DOWN");

            ///*----------TEST-1----------*/
            controller.battery.column_list[1].elevator_list[0].elevatorFloor = 20;
            controller.battery.column_list[1].elevator_list[0].elevator_direction = "UP";
            controller.battery.column_list[1].elevator_list[0].status = "MOVING";
            controller.battery.column_list[1].elevator_list[0].floor_list.Add(5);

            controller.battery.column_list[1].elevator_list[1].elevatorFloor = 3;
            controller.battery.column_list[1].elevator_list[1].elevator_direction = "UP";
            controller.battery.column_list[1].elevator_list[1].status = "MOVING";
            controller.battery.column_list[1].elevator_list[1].floor_list.Add(15);


            controller.battery.column_list[1].elevator_list[2].elevatorFloor = 13;
            controller.battery.column_list[1].elevator_list[2].elevator_direction = "DOWN";
            controller.battery.column_list[1].elevator_list[2].status = "MOVING";
            controller.battery.column_list[1].elevator_list[2].floor_list.Add(1);


            controller.battery.column_list[1].elevator_list[3].elevatorFloor = 15;
            controller.battery.column_list[1].elevator_list[3].elevator_direction = "DOWN";
            controller.battery.column_list[1].elevator_list[3].status = "MOVING";
            controller.battery.column_list[1].elevator_list[3].floor_list.Add(2);


            controller.battery.column_list[1].elevator_list[4].elevatorFloor = 6;
            controller.battery.column_list[1].elevator_list[4].elevator_direction = "DOWN";
            controller.battery.column_list[1].elevator_list[4].status = "MOVING";
            controller.battery.column_list[1].elevator_list[4].floor_list.Add(1);

            controller.AssignElevator(20);


            ///*----------TEST-2----------*/
            //controller.battery.column_list[2].elevator_list[0].elevatorFloor = 1;
            //controller.battery.column_list[2].elevator_list[0].elevator_direction = "UP";
            //controller.battery.column_list[2].elevator_list[0].status = "IDLE";
            //controller.battery.column_list[2].elevator_list[0].floor_list.Add(21);

            //controller.battery.column_list[2].elevator_list[1].elevatorFloor = 23;
            //controller.battery.column_list[2].elevator_list[1].elevator_direction = "UP";
            //controller.battery.column_list[2].elevator_list[1].status = "MOVING";
            //controller.battery.column_list[2].elevator_list[1].floor_list.Add(28);


            //controller.battery.column_list[2].elevator_list[2].elevatorFloor = 33;
            //controller.battery.column_list[2].elevator_list[2].elevator_direction = "DOWN";
            //controller.battery.column_list[2].elevator_list[2].status = "MOVING";
            //controller.battery.column_list[2].elevator_list[2].floor_list.Add(1);


            //controller.battery.column_list[2].elevator_list[3].elevatorFloor = 40;
            //controller.battery.column_list[2].elevator_list[3].elevator_direction = "DOWN";
            //controller.battery.column_list[2].elevator_list[3].status = "MOVING";
            //controller.battery.column_list[2].elevator_list[3].floor_list.Add(24);


            //controller.battery.column_list[2].elevator_list[4].elevatorFloor = 39;
            //controller.battery.column_list[2].elevator_list[4].elevator_direction = "DOWN";
            //controller.battery.column_list[2].elevator_list[4].status = "MOVING";
            //controller.battery.column_list[2].elevator_list[4].floor_list.Add(1);

            //controller.AssignElevator(36);
            //Elevator elevator = controller.requestElevator(1, 36);




            ///*----------TEST-3----------*/
            //controller.battery.column_list[3].elevator_list[0].elevatorFloor = 58;
            //controller.battery.column_list[3].elevator_list[0].elevator_direction = "DOWN";
            //controller.battery.column_list[3].elevator_list[0].status = "MOVING";
            //controller.battery.column_list[3].elevator_list[0].floor_list.Add(1);

            //controller.battery.column_list[3].elevator_list[1].elevatorFloor = 50;
            //controller.battery.column_list[3].elevator_list[1].elevator_direction = "UP";
            //controller.battery.column_list[3].elevator_list[1].status = "MOVING";
            //controller.battery.column_list[3].elevator_list[1].floor_list.Add(60);

            //controller.battery.column_list[3].elevator_list[2].elevatorFloor = 46;
            //controller.battery.column_list[3].elevator_list[2].elevator_direction = "UP";
            //controller.battery.column_list[3].elevator_list[2].status = "MOVING";
            //controller.battery.column_list[3].elevator_list[2].floor_list.Add(58);

            //controller.battery.column_list[3].elevator_list[3].elevatorFloor = 1;
            //controller.battery.column_list[3].elevator_list[3].elevator_direction = "up";
            //controller.battery.column_list[3].elevator_list[3].status = "MOVING";
            //controller.battery.column_list[3].elevator_list[3].floor_list.Add(54);

            //controller.battery.column_list[3].elevator_list[4].elevatorFloor = 60;
            //controller.battery.column_list[3].elevator_list[4].elevator_direction = "DOWN";
            //controller.battery.column_list[3].elevator_list[4].status = "MOVING";
            //controller.battery.column_list[3].elevator_list[4].floor_list.Add(1);

            //controller.AssignElevator(1);
            //Elevator elevator = controller.requestElevator(54, 1);




            /*----------TEST-4----------*/
            //controller.battery.column_list[0].elevator_list[0].elevatorFloor = -3;

            //controller.battery.column_list[0].elevator_list[1].elevatorFloor = 1;

            //controller.battery.column_list[0].elevator_list[2].elevatorFloor = -3;
            //controller.battery.column_list[0].elevator_list[2].elevator_direction = "DOWN";
            //controller.battery.column_list[0].elevator_list[2].status = "MOVING";
            //controller.battery.column_list[0].elevator_list[2].floor_list.Add(-5);


            //controller.battery.column_list[0].elevator_list[3].elevatorFloor = -6;
            //controller.battery.column_list[0].elevator_list[3].elevator_direction = "UP";
            //controller.battery.column_list[0].elevator_list[3].status = "MOVING";
            //controller.battery.column_list[0].elevator_list[3].floor_list.Add(1);


            //controller.battery.column_list[0].elevator_list[4].elevatorFloor = -1;
            //controller.battery.column_list[0].elevator_list[4].elevator_direction = "DOWN";
            //controller.battery.column_list[0].elevator_list[4].status = "MOVING";
            //controller.battery.column_list[0].elevator_list[4].floor_list.Add(-6);

            //Elevator elevator = controller.requestElevator(-2, 1);

        }
    }
}

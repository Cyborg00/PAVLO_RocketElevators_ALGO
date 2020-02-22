#START THE ELEVATOR SYSTEM */
import time

class elevatorController:
    def __init__(self, nbrOfFloors, nbrOfElevator):
        self.nbrOfFloors = nbrOfFloors
        self.nbrOfElevator = nbrOfElevator
        self.column = Column(nbrOfFloors, nbrOfElevator)
        print("START TEST")
#END START THE ELEVATOR SYSTEM */


#START     /* REQUEST AN ELEVATOR */                            
    def RequestElevator(self, FloorNumber, Direction):
        time.sleep(1)
        print(">>>>>>>>>>>>>>>>>>>>>>>>")
        print("REQUEST ELEVATOR TO THE FLOOR : ", FloorNumber)
        time.sleep(1)
        print(">>>>>>>>>>>>>>>>>>>>>>>>")
        print("CALL BUTTON LIGHT ON")
        time.sleep(1)
        elevator = self.find_best_elevator(FloorNumber, Direction)
        elevator.send_request(FloorNumber)
        return elevator
#END     /* REQUEST AN ELEVATOR */                            

#START     /* REQUESTING FLOOR */ 
    def RequestFloor(self, elevator, RequestedFloor):
        time.sleep(1)
        print(">>>>>>>>>>>>>>>>>>>>>>>>")
        print("REQUESTED FLOOR : ", RequestedFloor)
        time.sleep(1)
        print(">>>>>>>>>>>>>>>>>>>>>>>>")
        print("BUTTON LIGHT ON")
        time.sleep(1)
        elevator.send_request(RequestedFloor)
#END     /* REQUESTING FLOOR */ 


#START     /* LOOKING FOR THE BEST ELEVATOR */                            
    def find_best_elevator(self, FloorNumber, Direction):
        bestElevator = None
        shortestDistance = 1000
        for elevator in (self.column.elevator_list):
            if (FloorNumber == elevator.elevatorFloor and (elevator.status == "STOP" or elevator.status == "idle" or elevator.status == " MOVING")):
                return elevator
            else:
                referenceDisrance = abs(FloorNumber - elevator.elevatorFloor)
                if shortestDistance > referenceDisrance:
                    shortestDistance = referenceDisrance
                    bestElevator = elevator

                elif elevator.Direction == Direction:
                    bestElevator = elevator

        return bestElevator


class Elevator:
    def __init__(self, elevatorNumb, status, elevatorFloor, elevator_direction):
        self.elevatorNumb = elevatorNumb
        self.status = status
        self.elevatorFloor = elevatorFloor
        self.elevator_direction = elevator_direction
        self.floor_list = []
#END     /* LOOKING FOR THE BEST ELEVATOR */                            

#START     /* SENDING A REQUEST */                            

    def send_request(self, RequestedFloor):
        self.floor_list.append(RequestedFloor)
        self.compute_list()
        self.operate_elevator(RequestedFloor)
#END     /* SENDING A REQUEST */                            

#START     /* COMPUTING LIST */                            
    def compute_list(self):
        if self.elevator_direction == "UP":
            self.floor_list.sort()
        elif self.elevator_direction == "DOWN":
            self.floor_list.sort()
            self.floor_list.reverse()
        return self.floor_list
#END     /* COMPUTING LIST */                            

#START     /* OPERATING THE ELEVATOR */                            
    def operate_elevator(self, RequestedFloor):
        while (len(self.floor_list) > 0):
            if ((RequestedFloor == self.elevatorFloor)):
                self.Open_door()
                self.status = " MOVING"

                self.floor_list.pop()
            elif (RequestedFloor < self.elevatorFloor):

                self.status = " MOVING"
                print(">>>>>>>>>>>>>>>>>>>>>>>>")
                print("Elevator", self.elevatorNumb, self.status)
                print(">>>>>>>>>>>>>>>>>>>>>>>>")
                self.Direction = "DOWN"
                self.Move_down(RequestedFloor)
                self.status = "STOP"
                print(">>>>>>>>>>>>>>>>>>>>>>>>")
                print("Elevator", self.elevatorNumb, self.status)
                print(">>>>>>>>>>>>>>>>>>>>>>>>")
                self.Open_door()
                self.floor_list.pop()
            elif (RequestedFloor > self.elevatorFloor):

                time.sleep(1)
                self.status = " MOVING"
                print(">>>>>>>>>>>>>>>>>>>>>>>>")
                print("Elevator", self.elevatorNumb, self.status)
                print(">>>>>>>>>>>>>>>>>>>>>>>>")
                self.Direction = "UP"
                self.Move_up(RequestedFloor)
                self.status = "STOP"
                print(">>>>>>>>>>>>>>>>>>>>>>>>")
                print("Elevator", self.elevatorNumb, self.status)
                print(">>>>>>>>>>>>>>>>>>>>>>>>")

                self.Open_door()

                self.floor_list.pop()

        if self.floor_list == 0:
            self.status = "idle"
#END     /* OPERATING THE ELEVATOR */ 

#START     /* OPEN AND CLOSE DOORS BUTTONS */              
    def Open_door(self):
        time.sleep(1)
        print("OPENING THE DOOR")
        print(">>>>>>>>>>>>>>>>>>>>>>>>")
        print("BUTTON LIGHT OFF")
        time.sleep(1)
        print(">>>>>>>>>>>>>>>>>>>>>>>>")
        time.sleep(1)
        self.Close_door()

    def Close_door(self):
        print("CLOSING THE DOORS")
        time.sleep(1)
#END     /* OPEN AND CLOSE DOORS BUTTONS */              


#START     /* MOOVING THE ELEVATOR UP OR DOWN */
    def Move_up(self, RequestedFloor):
        print("FLOOR : ", self.elevatorFloor)
        time.sleep(1)
        while(self.elevatorFloor != RequestedFloor):
            self.elevatorFloor += 1
            print("FLOOR : ", self.elevatorFloor)
            time.sleep(1)

    def Move_down(self, RequestedFloor):
        print("FLOOR : ", self.elevatorFloor)
        time.sleep(1)
        while(self.elevatorFloor != RequestedFloor):
            self.elevatorFloor -= 1
            print("FLOOR : ", self.elevatorFloor)
            
            time.sleep(1)
#START     /* MOOVING THE ELEVATOR UP OR DOWN */                            


#START     /* CALL BUTTON */                            
class callButton:
    def __init__(self, FloorNumber, Direction):
        self.FloorNumber = FloorNumber
        self.Direction = Direction
        self.light = False
#END     /* CALLING BUTTON */                            


#START     /* FLOOR BUTTON */
class floorButton:
    def __init__(self, RequestedFloor):
        self.RequestedFloor = RequestedFloor
#START     /* FLOOR  BUTTON */


class Column:
    def __init__(self, nbrOfFloors, nbrOfElevator):
        self.nbrOfFloors = nbrOfFloors
        self.nbrOfElevator = nbrOfElevator
        self.elevator_list = []
        for i in range(nbrOfElevator):
            elevator = Elevator(i, "idle", 1, "UP")
            self.elevator_list.append(elevator)


#----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
# START THE ELEVATOR TEST */

controller = elevatorController(10, 2)
controller.column.elevator_list[0].elevatorFloor = 2
controller.column.elevator_list[0].status = " MOVING"
controller.column.elevator_list[0].elevator_direction = "DOWN"
controller.column.elevator_list[1].elevatorFloor = 6
controller.column.elevator_list[1].status = " MOVING"
controller.column.elevator_list[1].elevator_direction = "DOWN"

elevator = controller.RequestElevator(9, "UP")
controller.RequestFloor(elevator, 3)
print("TEST FINISHED")
# END THE ELEVATOR TEST */


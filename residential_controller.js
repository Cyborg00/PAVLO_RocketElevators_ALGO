//START   /* START THE ELEVATOR SYSTEM */
function startElevatorSystem(nbrOfFloors, nbrOfElevator) {
  var controller = new ElevatorController(nbrOfFloors, nbrOfElevator);
  return controller;
}
class Column {
  constructor(nbrOfFloors, nbrOfElevator) {
    this.nbrOfFloors = nbrOfFloors;
    this.nbrOfElevator = nbrOfElevator;
    this.elevatorList = [];
    for (let i = 0; i < this.nbrOfElevator; i++) {
      let elevator = new Elevator(i, "idle", 1, "UP");
      this.elevatorList.push(elevator);
    }
  }
}

class Elevator {
  constructor(elevatorId, status, currentFloor, currentDirection) {
    this.elevatorId = elevatorId;
    this.status = status;
    this.currentFloor = currentFloor;
    this.currentDirection = currentDirection;
    this.floorList = [];
  }
  //END     /* START THE ELEVATOR SYSTEM */


  //START     /* SEND THE REQUESTED FLOOR TO THE COMPUTE LIST */              
  sendRequest(RequestedFloor) {
    this.floorList.push(RequestedFloor);
    this.compute_list();
    this.operate_elevator(RequestedFloor);
  }

  compute_list() {
    if (this.currentDirection === "UP") {
      this.floorList.sort();
    } else if (this.currentDirection === "DOWN") {
      this.floorList.sort();
      this.floorList.reverse();
    }
    return this.floorList;
  }
  //END     /* SEND THE REQUESTED FLOOR TO THE COMPUTE LIST */              



  //START     /* SEND THE REQUESTED FLOOR TO THE OPERATE SYSTEM AND TAKE ACTION */                            
  operate_elevator(RequestedFloor) {
    while (this.floorList > 0) {
      if (RequestedFloor === this.currentFloor) {
        this.openDoor();
        this.status = "MOVING ... ";
        this.floorList.shift();
      } else if (RequestedFloor < this.currentFloor) {
        this.status = "MOVING ... ";
        console.log(">>>>>>>>>>>>>>>>>>>>>>>>");
        console.log("ELEVATOR " + this.elevatorId, this.status);
        console.log(">>>>>>>>>>>>>>>>>>>>>>>>");
        this.moveDown(RequestedFloor);
        this.status = "STOP";
        console.log(">>>>>>>>>>>>>>>>>>>>>>>>");
        console.log("ELEVATOR " + this.elevatorId, this.status);
        console.log(">>>>>>>>>>>>>>>>>>>>>>>>");
        this.openDoor();
        this.floorList.shift();
      } else if (RequestedFloor > this.currentFloor) {
        delay(1000);
        this.status = "MOVING ... ";
        console.log(">>>>>>>>>>>>>>>>>>>>>>>>");
        console.log("ELEVATOR " + this.elevatorId, this.status);
        console.log(">>>>>>>>>>>>>>>>>>>>>>>>");
        this.moveUp(RequestedFloor);
        this.status = "STOP";
        console.log(">>>>>>>>>>>>>>>>>>>>>>>>");
        console.log("ELEVATOR " + this.elevatorId, this.status);
        console.log(">>>>>>>>>>>>>>>>>>>>>>>>");

        this.openDoor();

        this.floorList.shift();
      }
    }
    if (this.floorList === 0) {
      this.status = "idle";
    }
  }
  requestFloorButton(RequestedFloor) {
    this.RequestedFloor = RequestedFloor;
    this.floorLight = floorLight;
  }
  callFloorButton(FloorNumber, Direction) {
    this.FloorNumber = FloorNumber;
    this.Direction = Direction;
  }
  //END     /* SEND THE REQUESTED FLOOR TO THE OPERATE SYSTEM AND TAKE ACTION */                            


  //START     /* OPEN AND CLOSE DOORS BUTTONS */              
  openDoor() {
    delay(1000);
    console.log("<-- OPEN DOOR -->");
    console.log(">>>>>>>>>>>>>>>>>>>>>>>>");
    console.log("BUTTON LIGHT OFF");
    delay(1000);

    console.log(">>>>>>>>>>>>>>>>>>>>>>>>");
    delay(1000);
    this.closeDoor();
  }
  closeDoor() {
    console.log("--> CLOSE DOOR <--");
    delay(1000);
  }
  //END     /* OPEN AND CLOSE DOORS BUTTONS */              


  //START     /* MOVE THE ELEVATOR UP */                            
  moveUp(RequestedFloor) {
    console.log("FLOOR : " + this.currentFloor);
    delay(1000);
    while (this.currentFloor !== RequestedFloor) {
      this.currentFloor += 1;
      console.log("FLOOR : " + this.currentFloor);

      delay(1000);
    }
  }
  //END     /* MOVE THE ELEVATOR UP */                            


  //START     /* MOVE THE ELEVATOR DOWN */                            
  moveDown(RequestedFloor) {
    console.log("FLOOR : " + this.currentFloor);
    delay(1000);
    while (this.currentFloor !== RequestedFloor) {
      this.currentFloor -= 1;
      console.log("FLOOR : " + this.currentFloor);

      delay(1000);
    }
  }
  //END     /* MOVE THE ELEVATOR DOWN */                            
}

class ElevatorController {
  constructor(nbrOfFloors, nbrOfElevator) {
    this.nbrOfFloors = nbrOfFloors;
    this.nbrOfElevator = nbrOfElevator;
    this.column = new Column(nbrOfFloors, nbrOfElevator);
    // console.log(this.column);

    console.log("START TEST");
  }

  //START     /* REQUEST AN ELEVATOR */                            
  RequestElevator(FloorNumber, Direction) {
    delay(1000);
    console.log(">>>>>>>>>>>>>>>>>>>>>>>>");
    console.log("REQUEST ELEVATOR TO THE FLOOR : ", FloorNumber);
    delay(1000);
    console.log(">>>>>>>>>>>>>>>>>>>>>>>>");
    console.log("CALL BUTTON LIGHT ON");
    delay(1000);

    let elevator = this.findBestElevator(FloorNumber, Direction);
    elevator.sendRequest(FloorNumber);
    return elevator;
  }
  //START   REQUEST AN ELEVATOR

  //START     /* REQUESTING THE FLOOR */                            
  RequestFloor(elevator, RequestedFloor) {
    delay(1000);
    console.log(">>>>>>>>>>>>>>>>>>>>>>>>");
    console.log("REQUESTED FLOOR : ", RequestedFloor);
    delay(1000);
    console.log(">>>>>>>>>>>>>>>>>>>>>>>>");
    console.log("BUTTON LIGHT ON");
    delay(1000);
    elevator.sendRequest(RequestedFloor);
  }
  //END     /* REQUESTING THE FLOOR */                            


  //START     /* LOOKING FOR THE BEST ELEVATOR */                            
  findBestElevator(FloorNumber, Direction) {
    console.log("LOOKING FOR THE BEST ELEVATOR", FloorNumber, Direction);

    let bestElevator = null;
    let bestDistance = 1000;
    for (let i = 0; i < this.column.elevatorList.length; i++) {
      let elevator = this.column.elevatorList[i];
      if (FloorNumber === elevator.currentFloor && (elevator.status === "STOP" || elevator.status === "idle" || elevator.status === "MOVING ... ")) {
        return elevator;
      } else {
        let referenceDistance = Math.abs(FloorNumber - elevator.currentFloor);
        if (bestDistance > referenceDistance) {
          bestDistance = referenceDistance;
          bestElevator = elevator;

          if (elevator.Direction === Direction) {
            bestElevator = elevator;
          }
        }
      }
    }
    return bestElevator;
    //END     /* LOOKING FOR THE BEST ELEVATOR */                            
  }
}
//START   REQUEST AN ELEVATOR


//START     /* DELAY TIME */   //https://stackoverflow.com/questions/1183872/put-a-delay-in-javascript                          
function delay(milliseconds) {
  var start = new Date().getTime();
  for (var i = 0; i < 1e7; i++) {
    if ((new Date().getTime() - start) > milliseconds) {
      break;
    }
  }
}
//END     /* DELAY TIME */   //https://stackoverflow.com/questions/1183872/put-a-delay-in-javascript                          


/*--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/* START THE ELEVATOR TEST */
function executeTest() {
  var controller = startElevatorSystem(1, 5);

  controller.column.elevatorList[0].currentFloor = 1;
  controller.column.elevatorList[0].status = "MOVING ... ";
  controller.column.elevatorList[0].currentDirection = "DOWN";
  controller.column.elevatorList[1].currentFloor = 5;
  controller.column.elevatorList[1].status = "MOVING ...";
  controller.column.elevatorList[1].currentDirection = "DOWN";



  var elevator = controller.RequestElevator(7, "UP");
  controller.RequestFloor(elevator, 6);
  elevator = controller.RequestElevator(3, "DOWN");
  controller.RequestFloor(elevator, 5);
  console.log("TEST FINISHED");
}
//END /* START THE ELEVATOR TEST */

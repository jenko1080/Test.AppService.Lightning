export interface BoundingBox {
   // schema: {"bottomLeft":{"latitude":-39.25,"longitude":140,"z":null,"m":null,"coordinateSystem":{"epsgId":4326,"id":"4326","name":"WGS84"},"isEmpty":false},"topRight":{"latitude":-33.75,"longitude":150,"z":null,"m":null,"coordinateSystem":{"epsgId":4326,"id":"4326","name":"WGS84"},"isEmpty":false}}
    bottomLeft: {
        latitude: number;
        longitude: number;
        z: null;
        m: null;
        coordinateSystem: {
            epsgId: number;
            id: string;
            name: string;   
        };
        isEmpty: boolean;
    };
    topRight: {
        latitude: number;
        longitude: number;
        z: null;
        m: null;
        coordinateSystem: {
            epsgId: number;
            id: string;
            name: string;   
        };
        isEmpty: boolean;
    };
  }
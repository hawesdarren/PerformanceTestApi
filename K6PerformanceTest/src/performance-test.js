import http from "k6/http";
import { check, group } from "k6";
import { getEnvironmentData } from "./helper.js";
import { htmlReport } from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js";
import { textSummary } from "https://jslib.k6.io/k6-summary/0.0.1/index.js";

const { endPoint } = getEnvironmentData(__ENV.TARGET_ENVIRONMENT);


export const options = {
    insecureSkipTLSVerify: true,
    thresholds: {
        http_req_failed: ["rate< 0.01"],
        http_req_duration: ["p(95)<600"],
        checks: ["rate>0.99"]
    },
    scenarios: {
        ramping_request_rate: {
            executor: "ramping-arrival-rate",
            startRate: 10,
            timeUnit: "1m",
            preAllocatedVUs: 10,
            maxVUs: 100,
            stages: [
                { target: 5000, duration: "2m" },   
                //{ target: 5000, duration: "10m" },  
            ],
            gracefulStop: "30s",
        },
    },
};

export default function(){
    try{
        group("Weather forecast", function (){
            const response = http.get(`${endPoint}WeatherForecast`, {
                tags: {
                    RequestTag: "Weather forecast request"
                },
            });
            check(response, {
                "status was 200": (response) => response.status === 200,
                "response contains data": (response) => JSON.parse(response.body) !== null
            })
        });
    } catch (error) {
        console.error(`An error has occurred: ${error}`);
    }
};

export function handleSummary(data) {
    return {
        stdout: textSummary(data, { indent: " ", enableColors: true }),
        "summary.html": htmlReport(data),
      
    };
};
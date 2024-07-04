export function getEnvironmentData(environmentName){
    const environmentData = JSON.parse(
        open(`./Data/${environmentName}.json`)
    );

    const endPoint = `${environmentData.baseUrl}/`

    return {
        endPoint
    }
}
import { getLightPostComplains } from "@/app/api/actions/complainActions";

export default async function TicketList() {
  const complains = await getLightPostComplains();

  console.log("Complains:", complains);

  return (
    <div className="flex flex-col items-center justify-start bg-gray-100">
      {complains.length > 0 ? (
        <div className="w-full max-w-4xl p-6 bg-white rounded-lg shadow-md">
          <div className="relative overflow-x-auto shadow-md sm:rounded-lg">
            <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
              <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                <tr>
                  <th scope="col" className="px-6 py-3">
                    ID
                  </th>
                  <th scope="col" className="px-6 py-3">
                    LP Number
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Name
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Status
                  </th>
                  <th scope="col" className="px-6 py-3">
                    ComplainDate
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Action
                  </th>
                </tr>
              </thead>
              <tbody>
                {complains.map((complain) => (
                  <tr
                    key={complain.id}
                    className="odd:bg-white odd:dark:bg-gray-900 even:bg-gray-50 even:dark:bg-gray-800 border-b dark:border-gray-700 border-gray-200"
                  >
                    <th
                      scope="row"
                      className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
                    >
                      {complain.id}
                    </th>
                    <td className="px-6 py-4">{complain.lightPostNumber}</td>
                    <td className="px-6 py-4">{complain.name}</td>
                    <td className="px-6 py-4">{complain.status}</td>
                    <td className="px-6 py-4">{complain.complainDate}</td>
                    <td className="px-6 py-4">
                      <a
                        href="#"
                        className="font-medium text-blue-600 dark:text-blue-500 hover:underline"
                      >
                        Create Ticket
                      </a>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      ) : (
        <div className="w-full max-w-4xl p-6 bg-white rounded-lg shadow-md">
          <h2 className="text-xl font-semibold text-gray-700">
            No Complains Found
          </h2>
          <p className="text-gray-500">
            It seems there are no complains to display at the moment.
          </p>
        </div>
      )}
    </div>
  );
}
